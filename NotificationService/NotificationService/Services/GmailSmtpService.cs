using Microsoft.Extensions.Options;
using NotificationService.Configurations;
using NotificationService.Services.Interfaces;
using Common.Models.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace NotificationService.Services
{
    public class GmailSmtpService : ISmtpService
    {
        private readonly ILogger<GmailSmtpService> _logger;
        private readonly GmailSmtpOptions _smtpOptions;

        public GmailSmtpService(ILogger<GmailSmtpService> logger, IOptions<GmailSmtpOptions> options) 
        {
            _logger = logger;
            _smtpOptions = options.Value;
        }

        public async Task<ExecutionResult> SendAsync(string recipientName, string recipientEmail, string subject, string html)
        {
            MimeMessage message = new();

            message.From.Add(new MailboxAddress(_smtpOptions.SenderName, _smtpOptions.SenderEmail));
            message.To.Add(new MailboxAddress(recipientName, recipientEmail));
            message.Subject = subject;
            BodyBuilder bodyBuilder = new() { HtmlBody = html };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.UseSsl);
                await client.AuthenticateAsync(_smtpOptions.UserEmail, _smtpOptions.Password);

                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }

            _logger.LogInformation($"A message has been sent to the user's {recipientEmail} email address. Message: {html}");

            return new(isSuccess: true);
        }
    }
}
