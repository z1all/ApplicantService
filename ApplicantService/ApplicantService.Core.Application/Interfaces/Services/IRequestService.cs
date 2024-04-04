using Common.Models;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface IRequestService
    {
        Task<ExecutionResult<bool>> CheckAdmissionStatusIsCloseAsync(Guid applicantId);
        Task<ExecutionResult<bool>> CheckManagerEditPermissionAsync(Guid applicantId, Guid managerId);
    }
}