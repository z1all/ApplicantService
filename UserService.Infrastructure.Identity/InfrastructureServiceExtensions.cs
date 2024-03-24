using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
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
            // Databases
            string? postgreConnectionString = configuration.GetConnectionString("PostgreConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(postgreConnectionString!));

            string? redisConnectionString = configuration.GetConnectionString("RedisConnection");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));

            // Services
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<ITokenDbService, TokenRedisService>();

            return services;
        }

        public static void AddAutoMigration(this IServiceProvider services)
        {
            using var dbContext = services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }

        public static void AddDatabaseSeed(this IServiceProvider services)
        {
            using var roleManager = services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            AppDbSeed.AddRoles(roleManager);
        }
    }
}
