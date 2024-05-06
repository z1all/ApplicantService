using Common.Models.Models;

namespace NotificationService.Services.Interfaces
{
    public interface ISmtpService
    {
        Task<ExecutionResult> SendAsync(string recipientName, string recipientEmail, string subject, string html);
    }
}
