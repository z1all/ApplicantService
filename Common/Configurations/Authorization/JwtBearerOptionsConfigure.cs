using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common.Configurations.Authorization
{
    public class JwtBearerOptionsConfigure(IOptions<JwtOptions> _jwtOptions) : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtOptions jwtOptions = _jwtOptions.Value;

        public void Configure(string? name, JwtBearerOptions options)
        {
            if (name == JwtBearerDefaults.AuthenticationScheme)
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
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                ClockSkew = new TimeSpan(0, 0, 30),
            };
        }
    }
}
