using UserService.Core.Application.DTOs;
using Common.Models.Models;

namespace UserService.Core.Application.Interfaces
{
    public interface IRequestService
    {
        Task<ExecutionResult> CheckPermissionsAsync(Guid applicantId, Guid? managerId);
        Task<ExecutionResult> CreateManagerAsync(Manager manager);
        Task<ExecutionResult> DeleteManagerAsync(Guid managerId);
    }
}
