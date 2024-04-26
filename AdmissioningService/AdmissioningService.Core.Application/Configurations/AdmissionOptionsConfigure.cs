using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AdmissioningService.Core.Application.Configurations
{
    public class AdmissionOptionsConfigure(IConfiguration configuration) : IConfigureOptions<AdmissionOptions>
    {
        private readonly string valueKey = "Admission";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(AdmissionOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
