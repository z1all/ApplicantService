using EasyNetQ.AutoSubscribe;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.EasyNetQAutoSubscriber
{
    public static class EasyNetQAutoSubscriberExtensions
    {
        public static void AddEasyNetQAutoSubscriber(this IServiceCollection services, string prefix)
        {
            services.AddSingleton<EasyNetQAutoSubscriber>();
            services.AddSingleton<AutoSubscriber>(provider =>
            {
                var subscriber = new AutoSubscriber(provider.GetRequiredService<IBus>(), prefix)
                {
                    AutoSubscriberMessageDispatcher = provider.GetRequiredService<EasyNetQAutoSubscriber>(),
                };

                return subscriber;
            });
        }

        public static void UseEasyNetQAutoSubscriber(this IServiceProvider services, Assembly assembly)
        {
            services.GetRequiredService<AutoSubscriber>().Subscribe([assembly]);
        }
    }
}
