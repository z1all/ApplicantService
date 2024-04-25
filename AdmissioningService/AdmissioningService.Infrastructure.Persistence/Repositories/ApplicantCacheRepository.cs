using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class ApplicantCacheRepository : BaseRepository<ApplicantCache, AppDbContext>, IApplicantCacheRepository
    {
        public ApplicantCacheRepository(AppDbContext dbContext) : base(dbContext) { }

        public override Task<ApplicantCache?> GetByIdAsync(Guid applicantId)
        {
            return _dbContext.ApplicantCaches
                .Include(applicant => applicant.AddedDocumentTypes)
                .FirstOrDefaultAsync(applicant => applicant.Id == applicantId);
        }
    }
}
