using Microsoft.Extensions.DependencyInjection;
using EasyNetQ.AutoSubscribe;

namespace Common.ServiceBus.EasyNetQAutoSubscriber
{
    public class EasyNetQAutoSubscriber : IAutoSubscriberMessageDispatcher
    {
        private readonly IServiceProvider provider;

        public EasyNetQAutoSubscriber(IServiceProvider provider)
        {
            this.provider = provider;
        }

        void IAutoSubscriberMessageDispatcher.Dispatch<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
        {
            using (var scope = provider.CreateScope())
            {
                var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
                consumer.Consume(message);
            }
        }

        async Task IAutoSubscriberMessageDispatcher.DispatchAsync<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
        {
            using (var scope = provider.CreateScope())
            {
                var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
                await consumer.ConsumeAsync(message);
            }
        }
    }
}
