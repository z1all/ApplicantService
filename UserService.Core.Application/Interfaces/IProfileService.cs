using UserService.Core.Application.DTOs;
using UserService.Core.Application.Models;

namespace UserService.Core.Application.Interfaces
{
    public interface IProfileService
    {
        Task<ExecutionResult> ChangePassword(ChangePasswordRequest changePassword, Guid userId);
        Task<ExecutionResult> ChangeEmail(ChangeEmailRequest changeEmail, Guid userId);
        Task<ExecutionResult> ChangeProfile(ChangeProfileRequest changeProfile, Guid userId);
        Task<ExecutionResult> CreateManager(CreateManagerRequest createManager);
        Task<ExecutionResult> DeleteManager(Guid managerId);
    }
}
