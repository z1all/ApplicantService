using Microsoft.AspNetCore.Identity;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Enums;
using UserService.Core.Application.Interfaces;
using UserService.Infrastructure.Identity.Services;

namespace UserService.Infrastructure.Persistence.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenDbService _tokenDbService;
        private readonly TokenHelperService _tokenHelperService;

        public AuthService(UserManager<IdentityUser> userManager, ITokenDbService tokenDbService, TokenHelperService tokenHelperService)
        {
            _userManager = userManager;
            _tokenDbService = tokenDbService;
            _tokenHelperService = tokenHelperService;
        }

        public async Task<TokenResponse> ApplicantRegistrationAsync(RegistrationDTO registrationDTO)
        {
            IdentityUser user = new()
            {
                UserName = registrationDTO.FullName,
                Email = registrationDTO.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registrationDTO.Password);
            if (!result.Succeeded)
            {
                throw new NotImplementedException();
            }

            result = await _userManager.AddToRoleAsync(user, Role.Applicant.ToString());
            if (!result.Succeeded)
            {
                throw new NotImplementedException();
            }

            (string accessToken, Guid tokenJTI) = await _tokenHelperService.GenerateJWTTokenAsync(user);
            string refreshToken = _tokenHelperService.GenerateRefreshToken();

            bool saveTokenResult = await _tokenDbService.SaveTokens(refreshToken, tokenJTI);
            if (!saveTokenResult)
            {
                throw new NotImplementedException();
            }

            return new TokenResponse()
            {
                Access = accessToken,
                Refresh = refreshToken
            };
        }

        public async Task<TokenResponse> ApplicantLoginAsync(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        public async Task LogoutAsync(Guid userId, Guid tokenJTI)
        {
            throw new NotImplementedException();
        }

        public async Task<TokenResponse> UpdateAccessTokenAsync(string refresh, string access)
        {
            throw new NotImplementedException();
        }
    }
}
