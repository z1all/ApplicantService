using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UserService.Infrastructure.Identity.Configurations
{
    internal class CustomJwtBearerOptionsConfigure(IOptions<JwtOptions> _jwtOptions) : IConfigureNamedOptions<JwtBearerOptions> 
    {
        private readonly JwtOptions jwtOptions = _jwtOptions.Value;

        public void Configure(string? name, JwtBearerOptions options)
        {
            if (name == CustomJwtBearerDefaults.CheckOnlySignature)
            {
                Configure(options);
            }
        }

        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                ClockSkew = new TimeSpan(0, 0, 30),
            };
        }
    }
}
