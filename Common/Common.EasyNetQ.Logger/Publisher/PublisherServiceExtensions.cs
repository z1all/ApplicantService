using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Common.ServiceBus.Configurations;
using Common.EasyNetQ.Logger.Configurations;

namespace Common.EasyNetQ.Logger.Publisher
{
    public static class PublisherServiceExtensions
    {
        public static IServiceCollection AddPublisherEasyNetQLogger(this IServiceCollection services, string serviceName, bool addEasyNetQBus = false)
        {
            if (addEasyNetQBus)
            {
                services.AddEasyNetQ();
            }

            services.ConfigureOptions<EasyNetQLoggerOptionsConfigure>();

            services.AddLogging(builder =>
            {
                IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();

                builder.AddProvider(new EasyNetQLoggerProvider(serviceName, serviceProvider));
            });

            return services;
        }
    }
}
