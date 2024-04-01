using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace UserService.Infrastructure.Identity.Configurations.Authorization
{
    public class CustomAuthorizationOptionsConfigure : IConfigureOptions<AuthorizationOptions>
    {
        public void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(CustomJwtBearerDefaults.CheckOnlySignature, p => p.RequireAuthenticatedUser());
        }
    }
}
