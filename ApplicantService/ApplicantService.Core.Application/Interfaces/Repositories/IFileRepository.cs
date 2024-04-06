using ApplicantService.Core.Domain;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task<DocumentFileInfo?> GetInfoByFileIdAndApplicantIdAsync(Guid documentFileId, Guid applicantId);
        Task<FileEntity?> GetFileAsync(DocumentFileInfo documentFileInfo);
        Task AddAsync(DocumentFileInfo documentFileInfo, FileEntity file);
        Task DeleteAllFromDocumentAsync(Guid documentId);
        Task DeleteAsync(DocumentFileInfo documentFileInfo);
    }
}
