using Common.Configurations.Extensions;

namespace DictionaryService.Presentation.Web
{
    public static class PresentationServiceExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddSwaggerConfigure();
            services.AddModalStateConfigure();

            return services;
        }
    }
}
