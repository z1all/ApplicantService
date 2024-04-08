using DictionaryService.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Infrastructure.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContext) : base(dbContext) { }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<EducationProgram> EducationPrograms { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<EducationDocumentType> EducationDocumentTypes { get; set; }
        public DbSet<UpdateStatus> UpdateStatuses { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // EducationProgram
            modelBuilder.Entity<EducationProgram>()
                .HasOne(educationProgram => educationProgram.Faculty)
                .WithMany(faculty => faculty.EducationPrograms)
                .HasForeignKey(educationProgram => educationProgram.FacultyId)
                .IsRequired();

            modelBuilder.Entity<EducationProgram>()
                .HasOne(educationProgram => educationProgram.EducationLevel)
                .WithMany()
                .HasForeignKey(educationProgram => educationProgram.EducationLevelId)
                .IsRequired();

            // EducationDocumentType
            modelBuilder.Entity<EducationDocumentType>()
                .HasOne(educationDocumentType => educationDocumentType.EducationLevel)
                .WithMany()
                .HasForeignKey(educationDocumentType => educationDocumentType.EducationLevelId)
                .IsRequired();

            modelBuilder.Entity<EducationDocumentType>()
               .HasMany(educationDocumentType => educationDocumentType.NextEducationLevels)
               .WithMany();

            // UpdateStatus
            modelBuilder.Entity<UpdateStatus>()
                .HasKey(updateStatus => updateStatus.Id);

            modelBuilder.Entity<UpdateStatus>()
                .HasAlternateKey(updateStatus => updateStatus.DictionaryType);
        }
    }
}
