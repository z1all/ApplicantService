using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UserService.Presentation.Web.Helpers
{
    internal static class ConcreteJwtTokenHelper
    {
        public static bool TryGetAccessTokenJTI(this HttpContext httpContext, out Guid accessTokenJTI)
        {
            Claim? accessTokenJTIClaim = httpContext.User.Claims.FirstOrDefault(clam => clam.Type == JwtRegisteredClaimNames.Jti);

            return Guid.TryParse(accessTokenJTIClaim?.Value, out accessTokenJTI);
        }
    }
}
