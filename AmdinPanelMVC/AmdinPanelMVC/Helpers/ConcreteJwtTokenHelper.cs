using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AmdinPanelMVC.Helpers
{
    public enum ValidateStatus
    {
        Valid = 0,
        TokenExpired = 1,
        NotValid = 2,
    }

    public static class ConcreteJwtTokenHelper
    {
        public static ValidateStatus Validate(string? jwtToken, IOptions<JwtBearerOptions> options, out ClaimsPrincipal? claims)
        {
            claims = null;

            if (jwtToken is null) return ValidateStatus.NotValid;

            JwtBearerOptions jwtOptions = options.Value;
            TokenValidationParameters validationParameters = jwtOptions.TokenValidationParameters;
            
            var validator = new JwtSecurityTokenHandler();
            if (validator.CanReadToken(jwtToken))
            {
                try
                {
                    claims = validator.ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);
                }
                catch (Exception ex)
                {
                    return ex switch
                    {
                        SecurityTokenExpiredException => ValidateStatus.TokenExpired,
                        _ => ValidateStatus.NotValid,
                    };
                }
            }

            return ValidateStatus.Valid;
        }
    }
}
