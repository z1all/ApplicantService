using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Common.API.Configurations.Authorization
{
    public class JwtOptionsConfigure(IConfiguration configuration) : IConfigureOptions<JwtOptions>
    {
        private readonly string valueKey = "JwtAuthentication";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(JwtOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}

