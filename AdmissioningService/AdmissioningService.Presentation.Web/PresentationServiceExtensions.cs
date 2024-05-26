using AdmissioningService.Presentation.Web.BackgroundServices;
using Common.API.Configurations;
using Common.EasyNetQ.Logger.Publisher;

namespace AdmissioningService.Presentation.Web
{
    public static class PresentationServiceExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddSwaggerConfigure();
            services.AddModalStateConfigure();
            services.AddPublisherEasyNetQLogger("AdmissioningService");

            services.AddScoped<AdmissionBackgroundListener>();
            services.AddScoped<DictionaryBackgroundListener>();

            return services;
        }
    }
}
