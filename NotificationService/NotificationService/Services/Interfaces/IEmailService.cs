using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Notifications;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Notifications;

namespace NotificationService.Services.Interfaces
{
    public interface IEmailService
    {
        Task<ExecutionResult> SendManagerAppointedNotificationAsync(ManagerAppointedNotification notification);
        Task<ExecutionResult> SendAdmissionStatusUpdatedNotificationAsync(AdmissionStatusUpdatedNotification notification);
        Task<ExecutionResult> SendManagerCreatedNotificationAsync(ManagerCreatedNotification notification);
    }
}
