using Common.ServiceBus.Configurations;
using Common.ServiceBus.EasyNetQAutoSubscriber;
using NotificationService.BackgroundServices;
using NotificationService.Configurations;
using NotificationService.Services;
using NotificationService.Services.Interfaces;
using System.Reflection;

namespace NotificationService
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddEasyNetQ();
            services.AddEasyNetQAutoSubscriber("ApplicantService");

            services.AddScoped<NotificationBackgroundListener>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ISmtpService, GmailSmtpService>();

            services.ConfigureOptions<GmailSmtpOptionsConfigure>();

            return services;
        }

        public static IServiceProvider UseService(this IServiceProvider services)
        {
            services.UseEasyNetQAutoSubscriber(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
