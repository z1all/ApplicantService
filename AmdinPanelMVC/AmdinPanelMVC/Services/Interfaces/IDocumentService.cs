using AmdinPanelMVC.DTOs;
using Common.Models.DTOs.Applicant;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<ExecutionResult<PassportInfo>> GetPassportAsync(Guid applicantId);
        Task<ExecutionResult> ChangePassportAsync(ChangePassportDTO changePassport, Guid managerId);

        Task<ExecutionResult<EducationDocumentInfo>> GetEducationDocumentAsync(Guid applicantId, Guid documentId);
        Task<ExecutionResult> ChangeEducationDocumentAsync(ChangeEducationDocumentDTO changeEducationDocument, Guid managerId);

        Task<ExecutionResult<List<ScanInfo>>> GetDocumentScansAsync(Guid applicantId, Guid documentId);
        Task<ExecutionResult<FileDTO>> GetScanAsync(Guid applicantId, Guid documentId, Guid scanId);
        Task<ExecutionResult> AddDocumentScansAsync(Guid applicantId, Guid documentId, FileDTO file, Guid managerId);
        Task<ExecutionResult> DeleteDocumentScansAsync(Guid applicantId, Guid documentId, Guid scanId, Guid managerId);
    }
}
