using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DictionaryService.Infrastructure.ExternalService.Configurations
{
    public class WebExternalOptionsConfigure(IConfiguration configuration) : IConfigureOptions<WebExternalOptions>
    {
        private readonly string valueKey = "WebExternalOptions";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(WebExternalOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}