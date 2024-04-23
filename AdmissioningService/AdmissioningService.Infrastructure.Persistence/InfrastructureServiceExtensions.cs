using AdmissioningService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdmissioningService.Infrastructure.Persistence
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Databases
            services.AddEntityFrameworkDbContext(configuration);

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
