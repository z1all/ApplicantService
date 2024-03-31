using Microsoft.Extensions.DependencyInjection;

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
    }
}
