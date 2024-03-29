namespace UserService.Core.Application.Interfaces
{
    public interface IServiceBusProvider
    {
        IRequestService Request { get; }
        INotificationService Notification { get; } 
    }
}
