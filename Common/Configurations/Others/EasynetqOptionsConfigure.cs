using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Common.Configurations.Others
{
    public class EasyNetQOptionsConfigure(IConfiguration configuration) : IConfigureOptions<EasynetqOptions>
    {
        private readonly string valueKey = "EasyNetQ";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(EasynetqOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
