using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Common.Configurations.Authorization;
using Common.Configurations.Others;
using EasyNetQ;

namespace Common.Configurations.Extensions
{
    public static class ConfigurationServiceExtensions
    {
        public static IServiceCollection AddModalStateConfigure(this IServiceCollection services)
        {
            services.ConfigureOptions<ModalStateOptionsConfigure>();

            return services;
        }

        public static IServiceCollection AddSwaggerConfigure(this IServiceCollection services)
        {
            services.ConfigureOptions<SwaggerGenOptionsConfigure>();

            return services;
        }

        public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services)
        {
            services.ConfigureOptions<AuthorizationOptionsConfigure>();
            services.ConfigureOptions<JwtBearerOptionsConfigure>();
            services.ConfigureOptions<JwtOptionsConfigure>(); 

            return services.AddAuthentication()
                    .AddJwtBearer();
        }

        public static IServiceCollection AddEasyNetQ(this IServiceCollection services)
        {
            services.ConfigureOptions<EasyNetQOptionsConfigure>();

            services.AddSingleton<IBus>(provider =>
            {
                var easynetqOptions = provider.GetRequiredService<IOptions<EasynetqOptions>>().Value;

                return RabbitHutch.CreateBus(easynetqOptions.ConnectionString, r => r.EnableSystemTextJson());
            });

            return services;
        }
    }
}
