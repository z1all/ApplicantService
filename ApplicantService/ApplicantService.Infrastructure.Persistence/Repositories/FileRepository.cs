using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using ApplicantService.Infrastructure.Persistence.Configuration;
using ApplicantService.Infrastructure.Persistence.Contexts;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly FileStorageOptions _fileStorageOptions;

        public FileRepository(AppDbContext dbContext, IOptions<FileStorageOptions> fileStorageOptions)
        {
            _dbContext = dbContext;
            _fileStorageOptions = fileStorageOptions.Value;
        }

        public async Task<DocumentFileInfo?> GetInfoByFileIdAndDocumentIdAsync(Guid documentFileId, Guid documentId)
        {
            return await _dbContext.FilesInfo
                .FirstOrDefaultAsync(documentFileInfo => documentFileInfo.Id == documentFileId && documentFileInfo.DocumentId == documentId);
        }

        public async Task<FileEntity?> GetFileAsync(DocumentFileInfo documentFileInfo)
        {
            string path = Path.Combine(_fileStorageOptions.StoragePath, documentFileInfo.PathName);
            if (File.Exists(path))
            {
                return new() { File = await File.ReadAllBytesAsync(path) };
            }

            return null;
        }

        public async Task AddAsync(DocumentFileInfo documentFile, FileEntity file)
        {
            documentFile.Id = Guid.NewGuid();
            Directory.CreateDirectory(_fileStorageOptions.StoragePath);
            string path = Path.Combine(_fileStorageOptions.StoragePath, documentFile.PathName);
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await fileStream.WriteAsync(file.File);
            }
            await _dbContext.FilesInfo.AddAsync(documentFile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAllFromDocumentAsync(Guid documentId)
        {
            List<DocumentFileInfo> documentFilesInfo = await _dbContext.FilesInfo
                .Where(fileInfo => fileInfo.DocumentId == documentId)
                .ToListAsync();

            foreach(var fileInfo in documentFilesInfo)
            {
                string path = Path.Combine(_fileStorageOptions.StoragePath, fileInfo.PathName);
                if(File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            _dbContext.RemoveRange(documentFilesInfo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(DocumentFileInfo documentFileInfo)
        {
            string path = Path.Combine(_fileStorageOptions.StoragePath, documentFileInfo.PathName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            _dbContext.Remove(documentFileInfo);
            await _dbContext.SaveChangesAsync();
        }
    }
}
