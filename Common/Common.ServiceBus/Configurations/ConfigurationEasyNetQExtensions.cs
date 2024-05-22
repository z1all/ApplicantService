using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using EasyNetQ;

namespace Common.ServiceBus.Configurations
{
    public static class ConfigurationEasyNetQExtensions
    {
        public static IServiceCollection AddEasyNetQ(this IServiceCollection services)
        {
            services.ConfigureOptions<EasyNetQOptionsConfigure>();

            services.AddSingleton(provider =>
            {
                var easynetqOptions = provider.GetRequiredService<IOptions<EasynetqOptions>>().Value;

                return RabbitHutch.CreateBus(easynetqOptions.ConnectionString, r => r.EnableSystemTextJson());
            });

            return services;
        }
    }
}
