using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Common.API.Configurations.Authorization;
using Common.API.Configurations.Others;

namespace Common.API.Configurations
{
    public static class ConfigurationApiExtensions
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
            
            services.AddJwtBearerOptions();

            return services.AddAuthentication()
                    .AddJwtBearer();
        }

        public static IServiceCollection AddJwtBearerOptions(this IServiceCollection services)
        {
            services.ConfigureOptions<JwtBearerOptionsConfigure>();
            services.ConfigureOptions<JwtOptionsConfigure>();

            return services;
        }
    }
}
