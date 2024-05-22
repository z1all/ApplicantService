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
        private readonly GmailSmtpOptions _smtpOptions;

        public GmailSmtpService(IOptions<GmailSmtpOptions> options) 
        {
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

            return new(isSuccess: true);
        }
    }
}
