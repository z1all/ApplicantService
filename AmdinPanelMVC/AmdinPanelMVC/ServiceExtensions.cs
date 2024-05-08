using Microsoft.AspNetCore.Authentication.Cookies;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.Services;
using Common.ServiceBus.Configurations;

namespace AmdinPanelMVC
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddScoped<IUserService, RpcUserService>();
            services.AddEasyNetQ();

            return services;
        }
    }

}
