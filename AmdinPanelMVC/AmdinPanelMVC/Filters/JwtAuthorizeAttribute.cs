using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using AmdinPanelMVC.Helpers;

namespace AmdinPanelMVC.Filters
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Cookies.TryGetTokens(out string? jwtToken, out string? refreshToken))
            {
                context.Result = new UnauthorizedResult();
            }

            var jwtBearerOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<JwtBearerOptions>>();

            ValidateStatus status = ConcreteJwtTokenHelper.Validate(jwtToken, jwtBearerOptions, out ClaimsPrincipal? userClaims);
            if (status == ValidateStatus.NotValid)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            else if(status == ValidateStatus.TokenExpired) 
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            userClaims?.Claims.ToList().ForEach(userClaim => context.HttpContext.User.Claims.Append(userClaim));
        }
    }
}
