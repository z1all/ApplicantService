using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AmdinPanelMVC.Helpers;
using Common.Models.Enums;

namespace AmdinPanelMVC.Filters
{
    public sealed class RequiredAuthorizeAttribute : JwtAuthorizeAttribute
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

        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool authorizationResult = await CheckAuthorizeAsync(context);
            if (!authorizationResult)
            {
                Unauthorized(context);
                return;
            }

            if (Roles is not null && !CheckRequiredRoles(context.HttpContext.User))
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
    }
}
