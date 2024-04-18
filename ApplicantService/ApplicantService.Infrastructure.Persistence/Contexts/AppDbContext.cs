using ApplicantService.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApplicantService.Infrastructure.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Applicant> Applicants { get; private set; }
        public DbSet<DocumentFileInfo> FilesInfo { get; private set; }
        public DbSet<EducationDocumentTypeCache> EducationDocumentTypesCache { get; private set; }
        public DbSet<Document> Documents { get; private set; }
        public DbSet<Passport> Passports { get; private set; }
        public DbSet<EducationDocument> EducationDocuments { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Applicant
            modelBuilder.Entity<Applicant>()
                .HasKey(applicant => applicant.Id);

            // Document
            modelBuilder.Entity<Document>()
                .HasOne(document => document.Applicant)
                .WithMany(applicant => applicant.Documents)
                .HasForeignKey(document => document.ApplicantId)
                .IsRequired();

            // DocumentFileInfo
            modelBuilder.Entity<DocumentFileInfo>()
                .HasOne(documentFileInfo => documentFileInfo.Document)
                .WithMany(documents => documents.FilesInfo)
                .HasForeignKey(documentFileInfo => documentFileInfo.DocumentId)
                .IsRequired();

            // EducationDocument
            modelBuilder.Entity<EducationDocument>()
                .HasOne(educationDocument => educationDocument.EducationDocumentType)
                .WithMany()
                .HasForeignKey(educationDocument => educationDocument.EducationDocumentTypeId)
                .IsRequired();

            // Others
            modelBuilder.Entity<Document>()
                .UseTptMappingStrategy();
        }
    }
}
