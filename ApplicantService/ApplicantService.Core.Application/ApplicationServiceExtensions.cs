using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Application.Interfaces.Services;
using ApplicantService.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicantService.Core.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicantProfileService, ApplicantProfileService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IRequestService, EasyNetQRequestService>();
            services.AddScoped<INotificationService, EasyNetQNotificationService>();

            return services;
        }
    }
}
