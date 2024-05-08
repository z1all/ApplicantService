using AmdinPanelMVC.DTOs;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IUserService
    {
        Task<ExecutionResult> GetProfileAsync(Guid userId);
        Task<ExecutionResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO login);
    }
}
