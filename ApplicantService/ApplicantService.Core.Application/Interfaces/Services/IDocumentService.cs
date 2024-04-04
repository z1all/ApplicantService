using ApplicantService.Core.Application.DTOs;
using Common.Models;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface IDocumentService
    {
        Task<ExecutionResult<List<DocumentInfo>>> GetApplicantDocuments(Guid applicantId);
        Task<ExecutionResult> DeleteApplicantDocument(Guid documentId, Guid applicantId);

        Task<ExecutionResult<PassportInfo>> GetApplicantPassport(Guid applicantId);
        Task<ExecutionResult> UpdateApplicantPassport(EditAddPassportInfo documentInfo, Guid applicantId);
        Task<ExecutionResult> AddApplicantPassport(EditAddPassportInfo documentInfo, Guid applicantId);

        Task<ExecutionResult<EducationDocumentInfo>> GetApplicantEducationDocument(Guid documentId, Guid applicantId);
        Task<ExecutionResult> UpdateApplicantEducationDocument(Guid documentId, Guid applicantId, EditAddEducationDocumentInfo documentInfo);
        Task<ExecutionResult> AddApplicantEducationDocument(Guid applicantId, EditAddEducationDocumentInfo documentInfo);
    }
}
