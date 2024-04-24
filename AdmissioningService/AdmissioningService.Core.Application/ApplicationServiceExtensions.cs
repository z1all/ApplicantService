using Microsoft.Extensions.DependencyInjection;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Services;

namespace AdmissioningService.Core.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAdmissionService, AdmissionService>();

            return services;
        }
    }
}
