using Microsoft.Extensions.DependencyInjection;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Application.Services;

namespace ApplicantService.Core.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUpdateDictionaryService, UpdateDictionaryService>();
            
            return services;
        }
    }
}
