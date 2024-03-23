using Microsoft.AspNetCore.Identity;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;

namespace UserService.Infrastructure.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<TokenResponse> LoginAsync(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponse> RegistrationAsync(RegistrationDTO registrationDTO)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponse> UpdateAccessTokenAsync(string refresh, string access)
        {
            throw new NotImplementedException();
        }
    }
}
