using Microsoft.EntityFrameworkCore;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class ApplicantCacheRepository : BaseWithBaseEntityRepository<ApplicantCache, AppDbContext>, IApplicantCacheRepository
    {
        public ApplicantCacheRepository(AppDbContext dbContext) : base(dbContext) { }

        public Task<ApplicantCache?> GetByIdWithDocumentTypeAndLevelsAsync(Guid applicantId)
        {
            return _dbContext.ApplicantCaches
                .Include(applicant => applicant.AddedDocumentTypes)
                    .ThenInclude(documentType => documentType.NextEducationLevel)
                .FirstOrDefaultAsync(applicant => applicant.Id == applicantId);
        }

        public Task<ApplicantCache?> GetByIdWithDocumentTypeAsync(Guid applicantId)
        {
            return _dbContext.ApplicantCaches
                .Include(applicant => applicant.AddedDocumentTypes)
                .FirstOrDefaultAsync(applicant => applicant.Id == applicantId);
        }
    }
}
