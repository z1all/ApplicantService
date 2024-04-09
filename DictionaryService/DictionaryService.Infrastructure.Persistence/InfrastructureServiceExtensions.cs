using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DictionaryService.Infrastructure.Persistence.Contexts;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Infrastructure.Persistence.Repositories;

namespace DictionaryService.Infrastructure.Persistence
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Databases
            services.AddEntityFrameworkDbContext(configuration);

            // Repositories
            services.AddScoped<IEducationDocumentTypeRepository, EducationDocumentTypeRepository>();
            services.AddScoped<IEducationLevelRepository, EducationLevelRepository>();
            services.AddScoped<IEducationProgramRepository, EducationProgramRepository>();
            services.AddScoped<IFacultyRepository, FacultyRepository>();
            services.AddScoped<IUpdateStatusRepository, UpdateStatusRepository>();

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

        public static void AddDatabaseSeed(this IServiceProvider services)
        {
            using var dbContext = services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            AppDbSeed.AddUpdateStatuses(dbContext);
        }
    }
}
