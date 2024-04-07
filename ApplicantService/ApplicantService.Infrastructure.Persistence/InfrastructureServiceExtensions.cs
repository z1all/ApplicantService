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

        public static void AddDatabaseSeed(this IServiceProvider services)
        {
            using var dbContext = services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            //Guid userId = Guid.NewGuid();

            //UserCache user = new()
            //{
            //    Id = userId,
            //    Email = "asdas@ads.com",
            //    FullName = "Bite",
            //};

            //Applicant applicant = new()
            //{
            //    Birthday = DateOnly.MinValue,
            //    Citizenship = "123",
            //    Gender = Core.Domain.Enums.Gender.male,
            //    PhoneNumber = "1233123",
            //    UserId = user.Id,
            //};

            //dbContext.UsersCache.Add(user);
            //dbContext.Applicants.Add(applicant);

            //dbContext.Passports.Add(new()
            //{
            //    ApplicantId = userId,
            //    ApplicantIdCache = user.Id,
            //    DocumentType = Core.Domain.Enums.DocumentType.Passport,
            //    BirthPlace = "1dsfg",
            //    IssuedByWhom = "fdgh",
            //    IssueYear = DateOnly.MaxValue,
            //    SeriesNumber = "123dfgdfg",
            //});

            //dbContext.SaveChanges();
        }
    }
}
