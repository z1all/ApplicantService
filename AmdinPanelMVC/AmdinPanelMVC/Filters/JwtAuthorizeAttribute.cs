using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using AmdinPanelMVC.Helpers;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.DTOs;
using Common.API.Helpers;
using Common.Models.Models;

namespace AmdinPanelMVC.Filters
{
    public abstract class JwtAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public string AuthenticationScheme { get; set; } = JwtBearerDefaults.AuthenticationScheme;

        public abstract Task OnAuthorizationAsync(AuthorizationFilterContext context);

        protected async Task<bool> CheckAuthorizeAsync(AuthorizationFilterContext context)
        {
            HttpContext httpContext = context.HttpContext;

            if (!httpContext.Request.Cookies.TryGetTokens(out string? jwtToken, out string? refreshToken))
            {
                return false;
            }

            var _jwtBearerOptions = httpContext.RequestServices.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
            var jwtOptions = _jwtBearerOptions.Get(AuthenticationScheme);

            (ValidateStatus status, ClaimsPrincipal? userClaims) = await ConcreteJwtTokenHelper.ValidateAsync(jwtToken, jwtOptions);
            if (userClaims is not null) httpContext.User = userClaims;

            switch (status)
            {
                case ValidateStatus.NotValid:
                case ValidateStatus.TokenExpired when !await TryUpdateTokensAsync(httpContext, refreshToken):
                    return false;
            }

            return true;
        }

        private async Task<bool> TryUpdateTokensAsync(HttpContext httpContext, string? refreshToken)
        {
            if (!(httpContext.TryGetUserId(out var userId) && httpContext.TryGetAccessTokenJTI(out var accessTokenJTI)))
            {
                return false;
            }

            var _userService = httpContext.RequestServices.GetRequiredService<IAuthService>();
            ExecutionResult<TokensResponseDTO> result = await _userService.UpdateAccessTokenAsync(refreshToken!, accessTokenJTI, userId);
            if (!result.IsSuccess)
            {
                return false;
            }

            TokensResponseDTO tokens = result.Result!;
            if (!ConcreteJwtTokenHelper.TryGetUserClaims(tokens.JwtToken, out ClaimsPrincipal userClaims))
            {
                return false;
            }

            httpContext.User = userClaims;

            httpContext.Response.Cookies.RemoveTokens();
            httpContext.Response.Cookies.SetTokens(tokens.JwtToken, tokens.RefreshToken);

            return true;
        }
    }
}
