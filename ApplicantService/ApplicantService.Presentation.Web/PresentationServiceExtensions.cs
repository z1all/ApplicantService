using Common.Configurations.Extensions;

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
