using LoggerService.Core.Application.Services;
using Common.EasyNetQ.Logger.Receiver;
using Common.EasyNetQ.Logger.Receiver.Interfaces;

namespace LoggerService.Presentation.Web
{
    public static class PresentationServiceExtensions
    {
        public static void AddPresentationServiceExtensions(this IServiceCollection services)
        {
            services.AddScoped<IReceiverService, LogService>();
            services.AddReceiverEasyNetQLogger("LoggerService");
        }

        public static void UsePresentationServiceExtensions(this IServiceProvider services)
        {
            services.UseReceiverEasyNetQLogger();
        }
    }
}
