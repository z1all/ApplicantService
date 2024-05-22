using AmdinPanelMVC.Services;
using AmdinPanelMVC.Services.Interfaces;
using Common.API.Configurations;
using Common.ServiceBus.Configurations;

namespace AmdinPanelMVC
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, RpcAuthService>();
            services.AddScoped<IAdminService, ServiceBusAdminService>();
            services.AddScoped<IAdmissionService, RpcAdmissionService>();
            services.AddScoped<IApplicantService, RpcApplicantService>();
            services.AddScoped<IDocumentService, RpcDocumentService>();
            services.AddScoped<IDictionaryService, RpcDictionaryService>();
            services.AddEasyNetQ();

            services.AddJwtBearerOptions();

            return services;
        }
    }
}
