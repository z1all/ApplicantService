using Common.Models.Models;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<ExecutionResult> AddedEducationDocumentTypeAsync(Guid applicantId, Guid documentTypeId);
        Task<ExecutionResult> ChangeEducationDocumentTypeAsync(Guid applicantId, Guid lastDocumentTypeId, Guid newDocumentTypeId);
        Task<ExecutionResult> DeletedEducationDocumentTypeAsync(Guid applicantId, Guid documentTypeId);
        Task<ExecutionResult> UpdatedApplicantInfoAsync(Guid applicantId);
    }
}
