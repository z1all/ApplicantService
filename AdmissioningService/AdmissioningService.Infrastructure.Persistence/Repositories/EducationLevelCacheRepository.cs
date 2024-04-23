using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class EducationLevelCacheRepository : BaseRepository<EducationLevelCache, AppDbContext>, IEducationLevelCacheRepository
    {
        public EducationLevelCacheRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
