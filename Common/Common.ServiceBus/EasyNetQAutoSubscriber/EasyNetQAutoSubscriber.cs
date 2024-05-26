using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EasyNetQ.AutoSubscribe;

namespace Common.ServiceBus.EasyNetQAutoSubscriber
{
    public class EasyNetQAutoSubscriber : IAutoSubscriberMessageDispatcher
    {
        private readonly ILogger<EasyNetQAutoSubscriber> _logger;
        private readonly IServiceProvider provider;

        public EasyNetQAutoSubscriber(ILogger<EasyNetQAutoSubscriber> logger, IServiceProvider provider)
        {
            _logger = logger;
            this.provider = provider;
        }

        void IAutoSubscriberMessageDispatcher.Dispatch<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
        {
            try
            {
                using (var scope = provider.CreateScope())
                {
                    var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
                    consumer.Consume(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknow error in EasyNetQAutoSubscriber");
            }
        }

        async Task IAutoSubscriberMessageDispatcher.DispatchAsync<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
        {
            try
            {
                using (var scope = provider.CreateScope())
                {
                    var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
                    await consumer.ConsumeAsync(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknow error in EasyNetQAutoSubscriber");
            }
        }
    }
}
