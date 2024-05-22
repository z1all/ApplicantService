using AdmissioningService.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace AdmissioningService.Infrastructure.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserCache> UserCaches { get; private set; }
        public DbSet<Manager> Managers { get; private set; }
        public DbSet<FacultyCache> FacultyCaches { get; private set; }
        public DbSet<EducationProgramCache> EducationProgramCaches { get; private set; }
        public DbSet<EducationLevelCache> EducationLevelCaches { get; private set; }
        public DbSet<EducationDocumentTypeCache> EducationDocumentTypeCaches { get; private set; }
        public DbSet<ApplicantCache> ApplicantCaches { get; private set; }
        public DbSet<ApplicantAdmission> ApplicantAdmissions { get; private set; }
        public DbSet<AdmissionProgram> AdmissionPrograms { get; private set; }
        public DbSet<AdmissionCompany> AdmissionCompanies { get; private set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Manager 
            modelBuilder.Entity<Manager>()
                .HasKey(manager => manager.Id);

            modelBuilder.Entity<Manager>()
                .HasOne(manager => manager.User)
                .WithOne()
                .HasForeignKey<Manager>(manager => manager.Id)
                .IsRequired();

            modelBuilder.Entity<Manager>()
                .HasOne(manager => manager.Faculty)
                .WithMany()
                .HasForeignKey(manager => manager.FacultyId)
                .IsRequired(false);

            // ApplicantAdmission
            modelBuilder.Entity<ApplicantAdmission>()
               .HasOne(applicantAdmission => applicantAdmission.Manager)
               .WithMany(manager => manager.ApplicantAdmissions)
               .HasForeignKey(applicantAdmission => applicantAdmission.ManagerId)
               .IsRequired(false);

            modelBuilder.Entity<ApplicantAdmission>()
               .HasOne(applicantAdmission => applicantAdmission.AdmissionCompany)
               .WithMany(admissionCompany => admissionCompany.ApplicantAdmissions)
               .HasForeignKey(applicantAdmission => applicantAdmission.AdmissionCompanyId)
               .IsRequired();

            modelBuilder.Entity<ApplicantAdmission>()
               .HasOne(applicantAdmission => applicantAdmission.Applicant)
               .WithMany(applicant => applicant.Admissions)
               .HasForeignKey(applicantAdmission => applicantAdmission.ApplicantId)
               .IsRequired();

            // Applicant
            modelBuilder.Entity<ApplicantCache>()
                .HasMany(applicant => applicant.AddedDocumentTypes)
                .WithMany();

            // EducationProgram
            modelBuilder.Entity<EducationProgramCache>()
                .HasOne(educationProgram => educationProgram.Faculty)
                .WithMany()
                .HasForeignKey(educationProgram => educationProgram.FacultyId)
                .IsRequired();

            modelBuilder.Entity<EducationProgramCache>()
               .HasOne(educationProgram => educationProgram.EducationLevel)
               .WithMany()
               .HasForeignKey(educationProgram => educationProgram.EducationLevelId)
               .IsRequired();

            // EducationDocumentType
            modelBuilder.Entity<EducationDocumentTypeCache>()
                .HasOne(educationDocumentType => educationDocumentType.EducationLevel)
                .WithMany()
                .HasForeignKey(educationDocumentType => educationDocumentType.EducationLevelId)
                .IsRequired();

            modelBuilder.Entity<EducationDocumentTypeCache>()
                .HasMany(educationDocumentType => educationDocumentType.NextEducationLevel)
                .WithMany();

            // AdmissionProgram
            modelBuilder.Entity<AdmissionProgram>()
                .HasKey(admissionProgram => new { admissionProgram.ApplicantAdmissionId, admissionProgram.EducationProgramId });

            modelBuilder.Entity<AdmissionProgram>()
                .HasOne(admissionProgram => admissionProgram.EducationProgram)
                .WithMany()
                .HasForeignKey(admissionProgram => admissionProgram.EducationProgramId)
                .IsRequired();

            modelBuilder.Entity<AdmissionProgram>()
                .HasOne(admissionProgram => admissionProgram.ApplicantAdmission)
                .WithMany(applicantAdmission => applicantAdmission.AdmissionPrograms)
                .HasForeignKey(admissionProgram => admissionProgram.ApplicantAdmissionId)
                .IsRequired();
        }
    }
}
