using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService;
using Common.ServiceBus.ServiceBusDTOs.FromUserService;

namespace NotificationService.Services.Interfaces
{
    public interface IEmailService
    {
        Task<ExecutionResult> SendManagerAppointedNotificationAsync(ManagerAppointedNotification notification);
        Task<ExecutionResult> SendAdmissionStatusUpdatedNotificationAsync(AdmissionStatusUpdatedNotification notification);
        Task<ExecutionResult> SendManagerCreatedNotificationAsync(ManagerCreatedNotification notification);
    }
}
