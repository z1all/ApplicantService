using Microsoft.AspNetCore.Identity;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Enums;
using UserService.Core.Application.Extensions;
using UserService.Core.Application.Interfaces;
using UserService.Core.Application.Models;
using UserService.Core.Domain.Entities;
using UserService.Infrastructure.Identity.Services;

namespace UserService.Infrastructure.Persistence.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly ITokenDbService _tokenDbService;
        private readonly TokenHelperService _tokenHelperService;

        public AuthService(UserManager<CustomUser> userManager, ITokenDbService tokenDbService, TokenHelperService tokenHelperService)
        {
            _userManager = userManager;
            _tokenDbService = tokenDbService;
            _tokenHelperService = tokenHelperService;
        }

        public async Task<ExecutionResult<TokenResponse>> ApplicantRegistrationAsync(RegistrationDTO registrationDTO)
        {
            CustomUser user = new()
            {
                FullName = registrationDTO.FullName,
                Email = registrationDTO.Email,
                UserName = $"{registrationDTO.FullName}_{Guid.NewGuid()}",
            };

            IdentityResult result = await _userManager.CreateAsync(user, registrationDTO.Password);
            if (!result.Succeeded)
            {
                return new(){ Errors = result.Errors.ToErrorDictionary() };
            }

            result = await _userManager.AddToRoleAsync(user, Role.Applicant.ToString());
            if (!result.Succeeded)
            {
                return new() { Errors = result.Errors.ToErrorDictionary() };
            }

            (string accessToken, Guid tokenJTI) = await _tokenHelperService.GenerateJWTTokenAsync(user);
            string refreshToken = _tokenHelperService.GenerateRefreshToken();

            bool saveTokenResult = await _tokenDbService.SaveTokens(refreshToken, tokenJTI);
            if (!saveTokenResult)
            {
                return new("unknowError", "Unknown error");
            }

            return new()
            {
                Result = new TokenResponse()
                {
                    Access = accessToken,
                    Refresh = refreshToken
                }
            };
        }

        public async Task<ExecutionResult<TokenResponse>> ApplicantLoginAsync(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecutionResult> LogoutAsync(Guid userId, Guid tokenJTI)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecutionResult<TokenResponse>> UpdateAccessTokenAsync(string refresh, string access)
        {
            throw new NotImplementedException();
        }
    }
}
