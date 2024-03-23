using UserService.Core.Application.DTOs;

namespace UserService.Core.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse> RegistrationAsync(RegistrationDTO registrationDTO);
        Task<TokenResponse> LoginAsync(LoginDTO loginDTO);
        Task LogoutAsync(Guid userId);
        Task<TokenResponse> UpdateAccessTokenAsync(string refresh, string access);
    }
}
