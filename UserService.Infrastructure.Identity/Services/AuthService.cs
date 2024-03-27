﻿using Microsoft.AspNetCore.Identity;
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
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly ITokenDbService _tokenDbService;
        private readonly TokenHelperService _tokenHelperService;

        public AuthService(UserManager<CustomUser> userManager, ITokenDbService tokenDbService, TokenHelperService tokenHelperService, SignInManager<CustomUser> signInManager)
        {
            _userManager = userManager;
            _tokenDbService = tokenDbService;
            _tokenHelperService = tokenHelperService;
            _signInManager = signInManager;
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

            return await GetTokensAsync(user);
        }

        public async Task<ExecutionResult<TokenResponse>> ApplicantLoginAsync(LoginDTO loginDTO)
        {
            CustomUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if(user == null)
            {
                return new("LoginFail", "Invalid email or password.");
            }
            
            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!signInResult.Succeeded)
            {
                return new("LoginFail", "Invalid email or password.");
            }

            IList<string> userRoles =  await _userManager.GetRolesAsync(user);
            if(!userRoles.Contains(Role.Applicant.ToString()))
            {
                return new("LoginFail", "Invalid email or password.");
            }

            return await GetTokensAsync(user);
        }

        public async Task<ExecutionResult> LogoutAsync(Guid accessTokenJTI)
        {
            bool result = await _tokenDbService.RemoveTokensAsync(accessTokenJTI);
            if(!result)
            {
                return new("LogoutFail", "The tokens have already been deleted.");
            }

            return new(true);
        }

        public async Task<ExecutionResult<TokenResponse>> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId)
        {
            bool tokenExist = await _tokenDbService.TokensExist(refresh, accessTokenJTI);
            if (!tokenExist)
            {
                return new("UpdateAccessTokenFail", "Tokens are not valid!");
            }

            bool removeResult = await _tokenDbService.RemoveTokensAsync(accessTokenJTI);
            if (!removeResult)
            {
                return new("UpdateAccessTokenFail", "Unknow error");
            }

            CustomUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new("UpdateAccessTokenFail", "Unknow error");
            }

            return await GetTokensAsync(user);
        }

        private async Task<ExecutionResult<TokenResponse>> GetTokensAsync(CustomUser user)
        {
            (string accessToken, Guid tokenJTI) = await _tokenHelperService.GenerateJWTTokenAsync(user);
            string refreshToken = _tokenHelperService.GenerateRefreshToken();

            bool saveTokenResult = await _tokenDbService.SaveTokensAsync(refreshToken, tokenJTI);
            if (!saveTokenResult)
            {
                return new("UnknowError", "Unknown error");
            }

            return new()
            {
                Result = new TokenResponse()
                {
                    Access = accessToken,
                    Refresh = refreshToken,
                }
            };
        }
    }
}
