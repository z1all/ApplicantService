using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ApplicantService.Infrastructure.Persistence.Configuration
{
    public class FileStorageOptionsConfigure(IConfiguration configuration) : IConfigureOptions<FileStorageOptions>
    {
        private readonly string valueKey = "FileSystem";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(FileStorageOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
