using AmdinPanelMVC.DTOs;
using Common.Models.DTOs.Admission;
using Common.Models.DTOs.User;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ExecutionResult<ManagerProfileDTO>> GetManagerProfileAsync(Guid managerId);
        Task<ExecutionResult> ChangeFullNameAsync(Guid userId, string newFullName, Guid? managerId = null);
        Task<ExecutionResult> ChangeEmailAsync(Guid userId, string newEmail);
        Task<ExecutionResult> ChangePasswordAsync(Guid userId, ChangePasswordDTO changePassword);
        Task<ExecutionResult<TokensResponseDTO>> LoginAsync(LoginRequestDTO login);
        Task<ExecutionResult> LogoutAsync(Guid accessTokenJTI);
        Task<ExecutionResult<TokensResponseDTO>> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId);
    }
}
