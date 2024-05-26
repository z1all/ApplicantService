using DictionaryService.Presentation.Web.BackgroundServices;
using Common.API.Configurations;
using Common.ServiceBus.Configurations;
using Common.EasyNetQ.Logger.Publisher;

namespace DictionaryService.Presentation.Web
{
    public static class PresentationServiceExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddSwaggerConfigure();
            services.AddEasyNetQ();
            services.AddPublisherEasyNetQLogger("DictionaryService");
            services.AddModalStateConfigure();

            services.AddScoped<UpdateDictionaryBackgroundListener>();

            return services;
        }
    }
}
