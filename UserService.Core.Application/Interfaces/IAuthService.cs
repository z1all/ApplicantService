using UserService.Core.Application.DTOs;
using UserService.Core.Application.Models;

namespace UserService.Core.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ExecutionResult<TokenResponse>> ApplicantRegistrationAsync(RegistrationDTO registrationDTO);
        Task<ExecutionResult<TokenResponse>> ApplicantLoginAsync(LoginDTO loginDTO);
        Task<ExecutionResult> LogoutAsync(Guid accessTokenJTI);
        Task<ExecutionResult<TokenResponse>> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId);
    }
}
