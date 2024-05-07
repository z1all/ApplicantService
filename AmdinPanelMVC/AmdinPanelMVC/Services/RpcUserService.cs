using AmdinPanelMVC.Models;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.Models;

namespace AmdinPanelMVC.Services
{
    public class RpcUserService : IUserService
    {
        public Task<ExecutionResult> GetProfileAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> LoginAsync(LoginViewModel login)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> LogoutAsync(Guid accessTokenJTI)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
