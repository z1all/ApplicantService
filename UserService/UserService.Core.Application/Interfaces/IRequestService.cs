using UserService.Core.Application.DTOs;
using Common.Models;

namespace UserService.Core.Application.Interfaces
{
    public interface IRequestService
    {
        Task<ExecutionResult> CreateManagerAsync(Manager manager);
        Task<ExecutionResult> DeleteManagerAsync(Guid managerId);
    }
}
