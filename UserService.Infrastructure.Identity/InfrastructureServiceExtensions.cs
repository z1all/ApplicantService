using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using UserService.Core.Application.Interfaces;
using UserService.Infrastructure.Identity;
using UserService.Infrastructure.Identity.Configurations;
using UserService.Infrastructure.Identity.Services;
using UserService.Infrastructure.Persistence.Contexts;
using UserService.Infrastructure.Persistence.Services;

namespace UserService.Infrastructure.Persistence
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Authorization
            services.AddJwtAuthorize();
            services.AddScoped<TokenHelperService>();

            // Databases
            services.AddEntityFrameworkDbContext(configuration);
            services.AddRedisDb(configuration);

            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<ITokenDbService, TokenRedisService>();

            // Options
            services.AddOptions();

            return services;
        }

        private static void AddJwtAuthorize(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();
        }

        private static void AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string? postgreConnectionString = configuration.GetConnectionString("PostgreConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(postgreConnectionString!));

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();
        }

        private static void AddRedisDb(this IServiceCollection services, IConfiguration configuration)
        {
            string? redisConnectionString = configuration.GetConnectionString("RedisConnection");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));
        }

        private static void AddOptions(this IServiceCollection services)
        {
            services.ConfigureOptions<JwtAuthorizeOptionsConfigure>();
            services.ConfigureOptions<JwtOptionsConfigure>();
            services.ConfigureOptions<IdentityOptionsConfigure>();
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
