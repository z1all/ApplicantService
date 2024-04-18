using DictionaryService.Presentation.Web.BackgroundServices;
using Common.API.Configurations;
using Common.Configurations;

namespace DictionaryService.Presentation.Web
{
    public static class PresentationServiceExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddSwaggerConfigure();
            services.AddEasyNetQ();
            services.AddModalStateConfigure();

            services.AddScoped<UpdateDictionaryBackgroundListener>();

            return services;
        }
    }
}
