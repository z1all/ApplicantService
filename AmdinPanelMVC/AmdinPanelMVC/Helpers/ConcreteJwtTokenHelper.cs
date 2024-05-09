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
                        ClaimsIdentity userIdentity = new (result.ClaimsIdentity.Claims, null);
                        return (ValidateStatus.TokenExpired, new ClaimsPrincipal(userIdentity));
                    }

                    return (ValidateStatus.Valid, new ClaimsPrincipal(result.ClaimsIdentity));
                }
            }

            return (ValidateStatus.NotValid, null);
        }

        public static bool TryGetUserClaims(string? jwtToken, out ClaimsPrincipal userClaims)
        {
            var validator = new JwtSecurityTokenHandler();
            if (validator.CanReadToken(jwtToken))
            {
                JwtSecurityToken jwtSecurityToken = validator.ReadJwtToken(jwtToken);

                var map = JwtSecurityTokenHandler.DefaultInboundClaimTypeMap;

                ClaimsIdentity userIdentity = new(jwtSecurityToken.Claims.Select(claim => 
                    new Claim(map.TryGetValue(claim.Type, out string? value) ? value : claim.Type, claim.Value)), 
                    "Bearer"
                );

                userClaims = new ClaimsPrincipal(userIdentity);

                return true;
            }

            userClaims = new();
            return false;    
        }
    }
}
