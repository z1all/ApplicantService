using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DictionaryService.Infrastructure.Persistence.Contexts;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Infrastructure.Persistence.Repositories;
using DictionaryService.Core.Application.Interfaces.Transaction;
using DictionaryService.Infrastructure.Persistence.Transaction;

namespace DictionaryService.Infrastructure.Persistence
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Databases
            services.AddEntityFrameworkDbContext(configuration);
            services.AddScoped<ITransactionProvider, EntityFrameworkTransactionProvider>();

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
            services.AddDbContext<UpdateStatusDbContext>(options => options.UseNpgsql(postgreConnectionString!));
        }

        public static void AddAutoMigration(this IServiceProvider services)
        {
            using(var appDbContext = services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>())
            {
                appDbContext.Database.Migrate();
            }

            using (var updateStatusDbContext = services.CreateScope().ServiceProvider.GetRequiredService<UpdateStatusDbContext>())
            {
                updateStatusDbContext.Database.Migrate();
            }
        }

        public static void AddDatabaseSeed(this IServiceProvider services)
        {
            using (var updateStatusDbContext = services.CreateScope().ServiceProvider.GetRequiredService<UpdateStatusDbContext>())
            {
                AppDbSeed.AddUpdateStatuses(updateStatusDbContext);
            }
        }
    }
}
