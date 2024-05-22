using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AmdinPanelMVC.Filters
{
    public sealed class RequiredUnauthorizeAttribute : JwtAuthorizeAttribute
    {
        public string Redirect { get; set; } = "/Home/Index";

        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool authorizationResult = await CheckAuthorizeAsync(context);
            if (authorizationResult)
            {
                context.Result = new RedirectResult(Redirect);
                return;
            }
        }
    }
}
