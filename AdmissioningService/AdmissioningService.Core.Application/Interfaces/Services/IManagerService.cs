using Common.Models.DTOs;
using Common.Models.Enums;
using Common.Models.Models;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface IManagerService
    {
        Task<ExecutionResult> CreateManagerAsync(Guid managerId, ManagerDTO createManager);
        Task<ExecutionResult> ChangeManagerAsync(Guid managerId, ManagerDTO changeManager);
        Task<ExecutionResult> DeleteManagerAsync(Guid managerId);
        Task<ExecutionResult> TakeApplicantAdmissionAsync(Guid admissionId, Guid managerId);
        Task<ExecutionResult> RefuseFromApplicantAdmissionAsync(Guid admissionId, Guid managerId);
        Task<ExecutionResult<List<ManagerProfileDTO>>> GetManagersAsync();
        Task<ExecutionResult<ManagerProfileDTO>> GetManagerAsync(Guid managerId);
        Task<ExecutionResult> ChangeApplicantAdmissionStatusAsync(Guid admissionId, ManagerChangeAdmissionStatus changeAdmissionStatus, Guid managerId);
    }
}
