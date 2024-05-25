using Microsoft.Extensions.DependencyInjection;
using Common.ServiceBus.Configurations;
using Common.ServiceBus.EasyNetQAutoSubscriber;
using System.Reflection;

namespace Common.EasyNetQ.Logger.Receiver
{
    public static class ReceiverServiceExtensions
    {
        public static IServiceCollection AddReceiverEasyNetQLogger(this IServiceCollection services, string prefix, bool addEasyNetQBus = true)
        {
            if (addEasyNetQBus)
            {
                services.AddEasyNetQ();
            }

            services.AddEasyNetQAutoSubscriber(prefix);
            services.AddScoped<ReceiverBackgroundListener>();

            return services;
        }

        public static IServiceProvider UseReceiverEasyNetQLogger(this IServiceProvider services)
        {
            services.UseEasyNetQAutoSubscriber(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
