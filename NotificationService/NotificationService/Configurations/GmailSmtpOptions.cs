namespace NotificationService.Configurations
{
    public class GmailSmtpOptions
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required bool UseSsl { get; set; }

        public required string UserEmail { get; set; }
        public required string Password { get; set; }

        public required string SenderName { get; set; }
        public required string SenderEmail { get; set; }
    }
}
