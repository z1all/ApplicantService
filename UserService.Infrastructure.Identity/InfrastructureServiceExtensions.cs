using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Core.Application.Interfaces;
using UserService.Infrastructure.Identity;
using UserService.Infrastructure.Identity.Services;
using UserService.Infrastructure.Persistence.Contexts;
using UserService.Infrastructure.Persistence.Services;

namespace UserService.Infrastructure.Persistence
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("PostgresConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProfileService, ProfileService>(); 
           
            return services;
        }

        public static void AddAutoMigration(this IServiceProvider services)
        {
            using var dbContext = services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }

        public static void AddEasynetq(this IServiceCollection services)
        {
            services.ConfigureOptions<EasynetqOptionsConfigure>();
            services.AddScoped<ISendNotification, EasynetqSendNotification>();
            services.AddSingleton<IBus>(provider =>
            {
                var easynetqOptions = provider.GetRequiredService<EasynetqOptions>();

                return RabbitHutch.CreateBus(easynetqOptions.ConnectionString);
            });
        }
    }
}
