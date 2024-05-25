using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using LoggerService.Infrastructure.Persistence.Contexts;
using LoggerService.Core.Application.Interfaces;
using LoggerService.Infrastructure.Persistence.Repositories;

namespace LoggerService.Infrastructure.Persistence
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Databases
            services.AddEntityFrameworkDbContext(configuration);

            // Repositories
            services.AddScoped<ILogRepository, LogRepository>();

            return services;
        }

        private static void AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string? postgreConnectionString = configuration.GetConnectionString("PostgreConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(postgreConnectionString!));
        }

        public static void AddAutoMigration(this IServiceProvider services)
        {
            using var dbContext = services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
