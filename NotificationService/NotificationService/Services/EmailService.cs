using NotificationService.Services.Interfaces;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Notifications;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Notifications;

namespace NotificationService.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISmtpService _smtpService;

        public EmailService(ISmtpService smtpService)
        { 
            _smtpService = smtpService;
        }

        public Task<ExecutionResult> SendAdmissionStatusUpdatedNotificationAsync(AdmissionStatusUpdatedNotification notification)
        {
            return _smtpService.SendAsync(notification.FullName, notification.Email, "Изменен статус поступления",
                $"<div>Здравствуйте, {notification.FullName}!</div>" +
                "<br />" +
                $"<div>Статус вашего поступления изменен на {notification.NewStatus}.</div>" +
                "<br />" +
                "<div>С уважением,</div>" +
                "<div>Команда ApplicantService!</div>");
        }

        public async Task<ExecutionResult> SendManagerAppointedNotificationAsync(ManagerAppointedNotification notification)
        {
            ExecutionResult resultManagerNotification = await _smtpService.SendAsync(
                notification.ManagerFullName, notification.ManagerEmail, "Назначен новый абитуриент",
                $"<div>Здравствуйте, {notification.ManagerFullName}!</div>" +
                "<br />" +
                "<div>Вам был назначен новый абитуриент.</div>" +
                "<br />" +
                "<div>С уважением,</div>" +
                "<div>Команда ApplicantService!</div>"
           );

            ExecutionResult resultApplicantNotification = await _smtpService.SendAsync(
                notification.ApplicantFullName, notification.ApplicantEmail, "Назначен менеджер на поступление",
                $"<div>Здравствуйте, {notification.ApplicantFullName}!</div>" +
                "<br />" +
                "<div>На ваше поступление был назначен менеджер.</div>" +
                "<br />" +
                "<div>С уважением,</div>" +
                "<div>Команда ApplicantService!</div>");

            if (!resultApplicantNotification.IsSuccess || !resultManagerNotification.IsSuccess)
            {
                return new(statusCode: StatusCodeExecutionResult.BadRequest, errors: resultApplicantNotification.Errors.AddRange(resultManagerNotification.Errors));
            }

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> SendManagerCreatedNotificationAsync(ManagerCreatedNotification notification)
        {
            return await _smtpService.SendAsync(
                notification.FullName, notification.Email, "Создан аккаунт менеджера",
                $"<div>Здравствуйте, {notification.Email}!</div>" +
                "<br />" +
                "<div>Для вас был создан аккаунт менеджера в системе ApplicantService.</div>" +
                $"<div>Ваш временный пароль: {notification.Password}. Обязательно поменяйте его!</div>" +
                "<div>Чтобы перейти в систему, перейдите по ссылке <a href=\"https://applicantserive/login\">Вход</a></div>" +
                "<br />" +
                "<div>С уважением,</div>" +
                "<div>Команда ApplicantService!</div>"
           );
        }
    }
}
