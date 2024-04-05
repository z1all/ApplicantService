using Microsoft.EntityFrameworkCore;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using ApplicantService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class EducationDocumentRepository : BaseRepository<EducationDocument, AppDbContext>, IEducationDocumentRepository
    {
        public EducationDocumentRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<EducationDocument?> GetByDocumentIdAndApplicantIdAsync(Guid documentId, Guid applicantId)
        {
            return await _dbContext.EducationDocuments
                .Include(document => document.EducationDocumentType)
                .Include(document => document.FilesInfo)
                .FirstOrDefaultAsync(document => document.Id == documentId && document.ApplicantId == applicantId);
        }

        public async Task<bool> AnyByDocumentIdAndApplicantIdAsync(Guid documentId, Guid applicantId)
        {
            return await _dbContext.EducationDocuments
               .AnyAsync(document => document.Id == documentId && document.ApplicantId == applicantId);
        }
    }
}
