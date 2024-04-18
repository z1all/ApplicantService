using Microsoft.Extensions.DependencyInjection;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Infrastructure.ExternalService.Services;
using DictionaryService.Infrastructure.ExternalService.Configurations;

namespace DictionaryService.Infrastructure.ExternalService  
{
    public static class InfrastructureExternalServiceExtensions
    {
        public static IServiceCollection AddInfrastructureExternalServices(this IServiceCollection services)
        {
            services.AddHttpClient<IExternalDictionaryService, ExternalDictionaryService>();
            services.ConfigureOptions<WebExternalOptionsConfigure>();

            return services;
        }
    }
}
