using Common.API.Configurations;
using Common.ServiceBus.Configurations;

namespace ApplicantService.Presentation.Web
{
    public static class PresentationServiceExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddSwaggerConfigure();
            services.AddEasyNetQ();

            services.AddScoped<BackgroundListener>();

            return services;
        }
    }
}
