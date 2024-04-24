using Microsoft.Extensions.DependencyInjection;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Services;
using AdmissioningService.Core.DictionaryHelpers;
using Common.ServiceBus.Configurations;

namespace AdmissioningService.Core.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAdmissionService, AdmissionService>();
            services.AddScoped<IRequestService, EasyNetQRequestService>();
            services.AddScoped<INotificationService, EasyNetQNotificationService>();
            services.AddEasyNetQ();
            
            services.AddScoped<DictionaryHelper>();

            return services;
        }
    }
}
