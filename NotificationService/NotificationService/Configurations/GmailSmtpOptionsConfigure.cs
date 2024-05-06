using Microsoft.Extensions.Options;

namespace NotificationService.Configurations
{
    public class GmailSmtpOptionsConfigure(IConfiguration configuration) : IConfigureOptions<GmailSmtpOptions>
    {
        private readonly string valueKey = "GmailSmtp";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(GmailSmtpOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
