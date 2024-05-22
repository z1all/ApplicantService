using Common.Models.Models;
using EasyNetQ;

namespace Common.ServiceBus.NotificationSender
{
    public abstract class NotificationSender
    {
        protected readonly IBus _bus;

        public NotificationSender(IBus bus)
        {
            _bus = bus;
        }

        protected ExecutionResult GiveResult(bool result, string errorMassage)
        {
            if (!result)
            {
                return new("SendNotificationFail", errorMassage);
            }
            return new(isSuccess: true);
        }

        protected async Task<bool> SendingHandler<T>(T notification) where T : class
        {
            return await _bus.PubSub
                .PublishAsync(notification)
                .ContinueWith(task => task.IsCompletedSuccessfully);
        }
    }
}
