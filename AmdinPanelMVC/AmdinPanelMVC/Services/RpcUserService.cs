using AmdinPanelMVC.DTOs;
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

        public async Task<ExecutionResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO login)
        {
            return new()
            {
                Result = new ()
                {
                    Id = Guid.NewGuid(),
                    Email = login.Email,
                    FullName = login.Password,
                }
            };
            return new("asdasd", "sadfsd");
            throw new NotImplementedException();
        }
    }
}
