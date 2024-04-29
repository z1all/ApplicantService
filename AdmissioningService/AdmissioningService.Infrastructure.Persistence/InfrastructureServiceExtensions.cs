using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using AdmissioningService.Infrastructure.Persistence.Repositories;
using AdmissioningService.Infrastructure.Persistence.StateMachines;
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

            // Repositories
            services.AddScoped<IUserCacheRepository, UserCacheRepository>();
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<IFacultyCacheRepository, FacultyCacheRepository>();
            services.AddScoped<IEducationProgramCacheRepository, EducationProgramCacheRepository>();
            services.AddScoped<IEducationLevelCacheRepository, EducationLevelCacheRepository>();
            services.AddScoped<IEducationDocumentTypeCacheRepository, EducationDocumentTypeCacheRepository>();
            services.AddScoped<IApplicantCacheRepository, ApplicantCacheRepository>();
            services.AddScoped<IApplicantAdmissionRepository, ApplicantAdmissionRepository>();
            services.AddScoped<IAdmissionProgramRepository, AdmissionProgramRepository>();
            services.AddScoped<IAdmissionCompanyRepository, AdmissionCompanyRepository>();

            // State machines
            services.AddScoped<IApplicantAdmissionStateMachin, ApplicantAdmissionStateMachin>();

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
