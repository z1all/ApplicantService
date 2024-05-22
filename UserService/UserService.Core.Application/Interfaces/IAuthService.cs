using UserService.Core.Application.DTOs;
using Common.Models.Models;

namespace UserService.Core.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ExecutionResult<TokensResponseDTO>> ApplicantRegistrationAsync(RegistrationDTO registrationDTO);
        Task<ExecutionResult<TokensResponseDTO>> ApplicantLoginAsync(LoginDTO loginDTO);
        Task<ExecutionResult<TokensResponseDTO>> ManagerLoginAsync(LoginDTO loginDTO);
        Task<ExecutionResult> LogoutAsync(Guid accessTokenJTI);
        Task<ExecutionResult<TokensResponseDTO>> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId);
    }
}
