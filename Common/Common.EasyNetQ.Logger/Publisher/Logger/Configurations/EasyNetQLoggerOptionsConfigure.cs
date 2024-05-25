using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Common.EasyNetQ.Logger.Configurations
{
    public class EasyNetQLoggerOptionsConfigure(IConfiguration configuration) : IConfigureOptions<EasyNetQLoggerOptions>
    {
        private readonly string valueKey = "Logging:EasyNetQLogLevel";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(EasyNetQLoggerOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
