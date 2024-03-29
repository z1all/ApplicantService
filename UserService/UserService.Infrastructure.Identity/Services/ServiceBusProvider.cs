using UserService.Core.Application.Interfaces;

namespace UserService.Infrastructure.Identity.Services
{
    internal class ServiceBusProvider : IServiceBusProvider
    {
        public IRequestService Request { get; private set; }
        public INotificationService Notification { get; private set; }

        public ServiceBusProvider(IRequestService request, INotificationService notification)
        {
            Request = request;
            Notification = notification;
        }
    }
}
