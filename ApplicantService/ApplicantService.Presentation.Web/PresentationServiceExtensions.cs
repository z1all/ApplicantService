using Common.API.Configurations;
using Common.ServiceBus.Configurations;
using Common.EasyNetQ.Logger.Publisher;

namespace ApplicantService.Presentation.Web
{
    public static class PresentationServiceExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddSwaggerConfigure();
            services.AddEasyNetQ();
            services.AddPublisherEasyNetQLogger("ApplicantService");

            services.AddModalStateConfigure();

            services.AddScoped<BackgroundListener>();

            return services;
        }
    }
}
