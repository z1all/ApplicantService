using UserService.Presentation.Web.Options;

namespace UserService.Presentation.Web
{
    public static class PresentationWebServiceExtensions
    {
        public static IServiceCollection AddPresentationWebService(this IServiceCollection services)
        {
            services.ConfigureOptions<ModalStateOptionsConfigure>();
            services.ConfigureOptions<SwaggerGenOptionsConfigure>();

            return services;
        }
    }
}
