using Microsoft.AspNetCore.Identity;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using UserService.Core.Application.Interfaces;
using UserService.Core.Domain.Entities;
using UserService.Infrastructure.Identity;
using UserService.Infrastructure.Identity.Services;
using UserService.Infrastructure.Persistence.Contexts;
using UserService.Infrastructure.Persistence.Services;
using Microsoft.Extensions.Options;
using UserService.Infrastructure.Identity.Configurations.Authorization;
using UserService.Infrastructure.Identity.Configurations.Other;

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
            services.AddEasyNetQ();

            return services;
        }

        private static void AddJwtAuthorize(this IServiceCollection services)
        {
            services.ConfigureOptions<JwtBearerOptionsConfigure>();
            services.ConfigureOptions<CustomJwtBearerOptionsConfigure>();
            services.ConfigureOptions<JwtOptionsConfigure>();
            services.ConfigureOptions<AuthorizationOptionsConfigure>();

            services.AddAuthentication()
                    .AddJwtBearer()
                    .AddJwtBearer(CustomJwtBearerDefaults.CheckOnlySignature);
        }

        private static void AddRedisDb(this IServiceCollection services, IConfiguration configuration)
        {
            string? redisConnectionString = configuration.GetConnectionString("RedisConnection");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));
        }

        private static void AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<IdentityOptionsConfigure>();

            string? postgreConnectionString = configuration.GetConnectionString("PostgreConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(postgreConnectionString!));

            services.AddIdentity<CustomUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();
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

        public static void AddEasyNetQ(this IServiceCollection services)
        {
            services.ConfigureOptions<EasynetqOptionsConfigure>();
            services.AddScoped<ISendNotification, EasynetqSendNotification>();
            services.AddSingleton<IBus>(provider =>
            {
                var easynetqOptions = provider.GetRequiredService<IOptions<EasynetqOptions>>().Value;

                return RabbitHutch.CreateBus(easynetqOptions.ConnectionString, s => s.EnableSystemTextJson());
            });
        }

        public static void AddEasyNetQSeed(this IServiceProvider services)
        {
            using var bus = services.CreateScope().ServiceProvider.GetRequiredService<IBus> ();
            EasyNetQSeed.AddQueue(bus.Advanced);
        }
    }
}
