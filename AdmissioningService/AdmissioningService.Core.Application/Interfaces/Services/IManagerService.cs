using AdmissioningService.Core.Application.DTOs;
using Common.Models.Models;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface IManagerService
    {
        Task<ExecutionResult> CreateManagerAsync(CreateManagerDTO createManager);
        Task<ExecutionResult> DeleteManagerAsync(Guid managerId);
        Task<ExecutionResult> TakeApplicantAdmissionAsync(Guid admissionId, Guid managerId);
        Task<ExecutionResult> RefuseFromApplicantAdmissionAsync(Guid admissionId, Guid managerId);
        Task<ExecutionResult<List<ManagerDTO>>> GetManagersAsync();
    }
}
