using UserService.Core.Application.DTOs;

namespace UserService.Core.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse> ApplicantRegistrationAsync(RegistrationDTO registrationDTO);
        Task<TokenResponse> ApplicantLoginAsync(LoginDTO loginDTO);
        Task LogoutAsync(Guid userId, Guid tokenJTI);
        Task<TokenResponse> UpdateAccessTokenAsync(string refresh, string access);
    }
}
