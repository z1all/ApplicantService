using Common.Models;
using ApplicantService.Core.Domain;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface IRequestService
    {
        Task<ExecutionResult> CheckAdmissionStatusIsCloseAsync(Guid applicantId);
        Task<ExecutionResult> CheckManagerEditPermissionAsync(Guid applicantId, Guid managerId);
        Task<ExecutionResult<EducationDocumentTypeCache>> GetEducationDocumentTypeAsync(Guid documentId);
    }
}