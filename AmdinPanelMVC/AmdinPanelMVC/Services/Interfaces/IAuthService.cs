using AmdinPanelMVC.DTOs;
using Common.Models.DTOs;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ExecutionResult<ManagerProfileDTO>> GetManagerProfileAsync(Guid managerId);
        Task<ExecutionResult> ChangeFullNameAsync(Guid managerId, string newFullName);
        Task<ExecutionResult> ChangeEmailAsync(Guid managerId, string newEmail);
        Task<ExecutionResult<TokensResponseDTO>> LoginAsync(LoginRequestDTO login);
        Task<ExecutionResult> LogoutAsync(Guid accessTokenJTI);
        Task<ExecutionResult<TokensResponseDTO>> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId);
    }
}
