using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        public static async Task<(ValidateStatus status, ClaimsPrincipal? userClaims)> ValidateAsync(string? jwtToken, JwtBearerOptions jwtOptions)
        {
            if (jwtToken is null) return (ValidateStatus.NotValid, null);

            TokenValidationParameters validationParameters = jwtOptions.TokenValidationParameters.Clone();
            bool validateLifetime = validationParameters.ValidateLifetime;
            validationParameters.ValidateLifetime = false;
            
            var validator = new JwtSecurityTokenHandler();
            if (validator.CanReadToken(jwtToken))
            {
                TokenValidationResult result = await validator.ValidateTokenAsync(jwtToken, validationParameters);

                if (result.IsValid)
                {
                    SecurityToken token = result.SecurityToken; 
                    if (validateLifetime && token.ValidTo < DateTime.UtcNow.Add(validationParameters.ClockSkew)) 
                    {
                        return (ValidateStatus.TokenExpired, new ClaimsPrincipal(result.ClaimsIdentity));
                    }

                    return (ValidateStatus.Valid, new ClaimsPrincipal(result.ClaimsIdentity));
                }
            }

            return (ValidateStatus.NotValid, null);
        }
    }
}
