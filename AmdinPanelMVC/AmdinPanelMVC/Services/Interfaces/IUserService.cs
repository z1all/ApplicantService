using AmdinPanelMVC.Models;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IUserService
    {
        Task<ExecutionResult> GetProfileAsync(Guid userId);
        Task<ExecutionResult> LoginAsync(LoginViewModel login);
        Task<ExecutionResult> LogoutAsync(Guid accessTokenJTI);
        Task<ExecutionResult> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId);
    }
}
