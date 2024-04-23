using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class ApplicantCacheRepository : BaseRepository<ApplicantCache, AppDbContext>, IApplicantCacheRepository
    {
        public ApplicantCacheRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
