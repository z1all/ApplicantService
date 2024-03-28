using UserService.Core.Application.DTOs;
using UserService.Core.Application.Models;

namespace UserService.Core.Application.Interfaces
{
    public interface ISendNotification
    {
        Task<ExecutionResult> CreatedApplicant(User user);
        Task<ExecutionResult> CreatedManager(User user);
        Task<ExecutionResult> UpdatedUser(User user);
    }
}
