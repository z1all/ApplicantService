using ApplicantService.Core.Application.DTOs;
using Common.Models.DTOs.Applicant;
using Common.Models.Models;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface IDocumentService
    {
        Task<ExecutionResult<List<DocumentInfo>>> GetApplicantDocumentsAsync(Guid applicantId);
        Task<ExecutionResult> DeleteApplicantDocumentAsync(Guid documentId, Guid applicantId, Guid? managerId = null);

        Task<ExecutionResult<PassportInfo>> GetApplicantPassportAsync(Guid applicantId);
        Task<ExecutionResult> UpdateApplicantPassportAsync(EditAddPassportInfo documentInfo, Guid applicantId, Guid? managerId = null);
        Task<ExecutionResult> AddApplicantPassportAsync(EditAddPassportInfo documentInfo, Guid applicantId, Guid? managerId = null);

        Task<ExecutionResult<EducationDocumentInfo>> GetApplicantEducationDocumentAsync(Guid documentId, Guid applicantId);
        Task<ExecutionResult> UpdateApplicantEducationDocumentAsync(Guid documentId, Guid applicantId, EditAddEducationDocumentInfo documentInfo, Guid? managerId = null);
        Task<ExecutionResult> AddApplicantEducationDocumentAsync(Guid applicantId, EditAddEducationDocumentInfo documentInfo, Guid? managerId = null);

        Task UpdateEducationDocumentType(UpdateEducationDocumentTypeDTO newDocumentType);
    }
}
