using NotificationService.Services.Interfaces;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService;
using Common.ServiceBus.ServiceBusDTOs.FromUserService;
using EasyNetQ.AutoSubscribe;

namespace NotificationService.BackgroundServices
{
    public class NotificationBackgroundListener :
        IConsumeAsync<ManagerAppointedNotification>, IConsumeAsync<AdmissionStatusUpdatedNotification>,
        IConsumeAsync<ManagerCreatedNotification>
    {
        private readonly IEmailService _emailService;

        public NotificationBackgroundListener(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task ConsumeAsync(ManagerAppointedNotification message, CancellationToken cancellationToken = default)
        {
            await _emailService.SendManagerAppointedNotificationAsync(message);
        }

        public async Task ConsumeAsync(AdmissionStatusUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            await _emailService.SendAdmissionStatusUpdatedNotificationAsync(message);
        }

        public async Task ConsumeAsync(ManagerCreatedNotification message, CancellationToken cancellationToken = default)
        {
            await _emailService.SendManagerCreatedNotificationAsync(message);
        }
    }
}
