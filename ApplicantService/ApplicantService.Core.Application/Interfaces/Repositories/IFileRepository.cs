using ApplicantService.Core.Domain;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task<DocumentFileInfo?> GetInfoByFileIdAndDocumentIdAsync(Guid documentFileId, Guid documentId);
        Task<FileEntity?> GetFileAsync(DocumentFileInfo documentFileInfo);
        Task AddAsync(DocumentFileInfo documentFileInfo, FileEntity file);
        Task DeleteAllFromDocumentAsync(Guid documentId);
        Task DeleteAsync(DocumentFileInfo documentFileInfo);
    }
}
