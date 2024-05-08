using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using AmdinPanelMVC.Helpers;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.DTOs;
using Common.API.Helpers;
using Common.Models.Models;
using Common.Models.Enums;

namespace AmdinPanelMVC.Filters
{
    public class JwtAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// Пользователь должен иметь хотя бы одну роль из списка.
        /// Стандартное значение свойства равно [Role.MainManager, Role.Manager, Role.Admin]
        /// </summary>
        public List<string> Roles { get; set; } = [Role.MainManager, Role.Manager, Role.Admin];
        /// <summary>
        /// Если проверка на Roles не прошла, то перенаправляет пользователя на страницу Redirect
        /// Стандартное значение свойства равно "/"
        /// </summary>
        public string Redirect { get; set; } = "/";
        public string AuthenticationScheme { get; set; } = JwtBearerDefaults.AuthenticationScheme;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            HttpContext httpContext = context.HttpContext;

            if (!httpContext.Request.Cookies.TryGetTokens(out string? jwtToken, out string? refreshToken))
            {
                Unauthorized(context);
                return;
            }

            var _jwtBearerOptions = httpContext.RequestServices.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
            var jwtOptions = _jwtBearerOptions.Get(AuthenticationScheme);

            (ValidateStatus status, ClaimsPrincipal? userClaims) = await ConcreteJwtTokenHelper.ValidateAsync(jwtToken, jwtOptions);
            if(userClaims is not null) httpContext.User.AddIdentities(userClaims.Identities);
            
            switch (status)
            {
                case ValidateStatus.NotValid:
                case ValidateStatus.TokenExpired when !await TryUpdateTokensAsync(httpContext, refreshToken, userClaims):
                    Unauthorized(context);
                    return;
            }

            bool temp = CheckRequiredRoles(userClaims);
            if (Roles is not null && !temp)
            {
                context.Result = new RedirectResult(Redirect);
                return;
            }
        }

        private bool CheckRequiredRoles(ClaimsPrincipal? userClaims)
        {
            return Roles.Any(role => userClaims?.IsInRole(role) ?? false);
        }

        private void Unauthorized(AuthorizationFilterContext context)
        {
            context.HttpContext.Response.Cookies.RemoveTokens();
            context.Result = new RedirectResult("/User/Login");
        }

        private async Task<bool> TryUpdateTokensAsync(HttpContext httpContext, string? refreshToken, ClaimsPrincipal? userClaims)
        {
            var _userService = httpContext.RequestServices.GetRequiredService<IAuthService>();

            if (httpContext.TryGetUserId(out var userId) && httpContext.TryGetAccessTokenJTI(out var accessTokenJTI))
            {
                ExecutionResult<TokensResponseDTO> result = await _userService.UpdateAccessTokenAsync(refreshToken!, accessTokenJTI, userId);
                if (result.IsSuccess)
                {
                    TokensResponseDTO tokens = result.Result!;

                    httpContext.Response.Cookies.RemoveTokens();
                    httpContext.Response.Cookies.SetTokens(tokens.JwtToken, tokens.RefreshToken);

                    return true;
                }
            }

            return false;
        }
    }
}
