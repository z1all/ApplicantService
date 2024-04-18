using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using EasyNetQ.AutoSubscribe;
using EasyNetQ;

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
