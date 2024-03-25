using UserService.Core.Application.DTOs;
using UserService.Core.Application.Models;

namespace UserService.Core.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ExecutionResult<TokenResponse>> ApplicantRegistrationAsync(RegistrationDTO registrationDTO);
        Task<ExecutionResult<TokenResponse>> ApplicantLoginAsync(LoginDTO loginDTO);
        Task<ExecutionResult> LogoutAsync(Guid userId, Guid tokenJTI);
        Task<ExecutionResult<TokenResponse>> UpdateAccessTokenAsync(string refresh, string access);
    }
}
