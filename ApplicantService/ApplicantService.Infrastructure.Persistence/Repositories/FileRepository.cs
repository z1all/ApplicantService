using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using ApplicantService.Infrastructure.Persistence.Contexts;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _dbContext;

        public FileRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<DocumentFileInfo?> GetInfoByFileIdAndApplicantIdAsync(Guid documentFileId, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<FileEntity?> GetFileAsync(DocumentFileInfo documentFileInfo)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(DocumentFileInfo documentFile, FileEntity file)
        {
            string path = documentFile.PathName;
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await fileStream.WriteAsync(file.File);
            }
            await _dbContext.FilesInfo.AddAsync(documentFile);
            await _dbContext.SaveChangesAsync();
        }

        public Task DeleteAllFromDocumentAsync(Guid documentId)
        {
            // Удаляем файлы, а потом инфу о них
            throw new NotImplementedException();
        }

        public Task DeleteAsync(DocumentFileInfo documentFileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
