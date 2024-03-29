using Common.Models;
using UserService.Core.Application.DTOs;

namespace UserService.Core.Application.Interfaces
{
    public interface IRequestService
    {
        Task<ExecutionResult> CreateManagerAsync(Manager manager);
        Task<ExecutionResult> DeleteManagerAsync(Guid managerId);
    }
}
