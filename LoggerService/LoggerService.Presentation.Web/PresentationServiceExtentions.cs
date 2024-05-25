using LoggerService.Core.Application.Interfaces;
using LoggerService.Core.Application.Services;
using Common.EasyNetQ.Logger.Receiver;
using Common.EasyNetQ.Logger.Receiver.Interfaces;
using Common.API.Configurations;

namespace LoggerService.Presentation.Web
{
    public static class PresentationServiceExtensions
    {
        public static void AddPresentationServiceExtensions(this IServiceCollection services)
        {
            services.AddScoped<IReceiverService, LogService>();
            services.AddScoped<ILogService, LogService>();
            services.AddReceiverEasyNetQLogger("LoggerService");

            services.AddJwtAuthentication();
            services.AddSwaggerConfigure();
            services.AddModalStateConfigure();
        }

        public static void UsePresentationServiceExtensions(this IServiceProvider services)
        {
            services.UseReceiverEasyNetQLogger();
        }
    }
}
