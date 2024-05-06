using UserService.Core.Application.DTOs;
using Common.Models.Models;

namespace UserService.Core.Application.Interfaces
{
    public interface IProfileService
    {
        Task<ExecutionResult> ChangePasswordAsync(ChangePasswordRequest changePassword, Guid userId);
        Task<ExecutionResult> ChangeEmailAsync(ChangeEmailRequest changeEmail, Guid userId, Guid? managerId = null);
        Task<ExecutionResult> ChangeProfileAsync(ChangeProfileRequest changeProfile, Guid userId, Guid? managerId = null);
        Task<ExecutionResult> CreateAdminAsync(CreateAdminRequest createAdmin);
        Task<ExecutionResult> CreateManagerAsync(CreateManagerRequest createManager);
        Task<ExecutionResult> DeleteManagerAsync(Guid managerId);
    }
}
