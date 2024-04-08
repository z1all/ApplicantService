using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Infrastructure.Persistence.Configuration;
using ApplicantService.Infrastructure.Persistence.Contexts;
using ApplicantService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicantService.Infrastructure.Persistence
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Databases
            services.AddEntityFrameworkDbContext(configuration);

            // Repositories
            services.AddScoped<IApplicantRepository, ApplicantRepository>();
            services.AddScoped<IEducationDocumentTypeCacheRepository, EducationDocumentTypeCacheRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IEducationDocumentRepository, EducationDocumentRepository>();
            services.AddScoped<IPassportRepository, PassportRepository>();

            // Configurations
            services.ConfigureOptions<FileStorageOptionsConfigure>();

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
