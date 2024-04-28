using Microsoft.Extensions.DependencyInjection;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Services;
using AdmissioningService.Core.Application.Configurations;
using Common.ServiceBus.Configurations;
using AdmissioningService.Core.Application.Helpers;

namespace AdmissioningService.Core.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAdmissionService, AdmissionService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IAdmissionBackgroundService, AdmissionBackgroundService>();
            services.AddScoped<IDictionaryBackgroundService, DictionaryBackgroundService>();
            services.AddScoped<IRequestService, EasyNetQRequestService>();
            services.AddScoped<INotificationService, EasyNetQNotificationService>();
            services.AddEasyNetQ();
            
            services.AddScoped<DictionaryHelper>();
            services.AddScoped<AdmissionHelper>();

            services.ConfigureOptions<AdmissionOptionsConfigure>();

            return services;
        }
    }
}
