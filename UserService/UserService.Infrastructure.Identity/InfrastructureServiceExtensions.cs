using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Core.Application.Interfaces;
using UserService.Core.Domain.Entities;
using UserService.Infrastructure.Identity.Services;
using UserService.Infrastructure.Identity.Contexts;
using UserService.Infrastructure.Identity.Configurations.Authorization;
using UserService.Infrastructure.Identity.Configurations.Other;
using Common.API.Configurations;
using Common.ServiceBus.Configurations;
using Common.EasyNetQ.Logger.Publisher;
using EasyNetQ;
using StackExchange.Redis;

namespace UserService.Infrastructure.Identity
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
            services.AddEasyNetQServices();
            services.AddPublisherEasyNetQLogger("UserService");

            return services;
        }

        private static void AddJwtAuthorize(this IServiceCollection services)
        {
            services.ConfigureOptions<CustomJwtBearerOptionsConfigure>();
            services.ConfigureOptions<CustomAuthorizationOptionsConfigure>();

            services.AddJwtAuthentication()
                .AddJwtBearer(CustomJwtBearerDefaults.CheckOnlySignature);         
        }

        private static void AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<IdentityOptionsConfigure>();

            string? postgreConnectionString = configuration.GetConnectionString("PostgreConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(postgreConnectionString!));

            services.AddIdentity<CustomUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();
        }

        private static void AddRedisDb(this IServiceCollection services, IConfiguration configuration)
        {
            string? redisConnectionString = configuration.GetConnectionString("RedisConnection");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));
        }

        private static void AddEasyNetQServices(this IServiceCollection services)
        {
            services.AddSingleton<IRequestService, EasyNetQRequestService>();
            services.AddSingleton<INotificationService, EasyNetQNotificationService>();
            services.AddSingleton<IServiceBusProvider, ServiceBusProvider>();
            services.AddEasyNetQ();
        }

        public static void AddAutoMigration(this IServiceProvider services)
        {
            using var dbContext = services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }

        public static void AddDatabaseSeed(this IServiceProvider services)
        {
            using (var roleManager = services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>())
            {
                AppDbSeed.AddRoles(roleManager);
            }

            using (var userManager = services.CreateScope().ServiceProvider.GetRequiredService<UserManager<CustomUser>>())
            {
                var profileService = services.CreateScope().ServiceProvider.GetRequiredService<IProfileService>();
                AppDbSeed.AddAdmins(profileService, userManager);
            }   
        }

        public static void AddEasyNetQSeed(this IServiceProvider services)
        {
            using var bus = services.CreateScope().ServiceProvider.GetRequiredService<IBus>();
            EasyNetQSeed.AddQueue(bus.Advanced);
        }
    }
}