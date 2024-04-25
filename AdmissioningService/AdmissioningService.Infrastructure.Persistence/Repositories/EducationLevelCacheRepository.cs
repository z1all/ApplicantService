using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class EducationLevelCacheRepository : BaseRepository<EducationLevelCache, AppDbContext>, IEducationLevelCacheRepository
    {
        public EducationLevelCacheRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<EducationLevelCache>> GetAllAsync()
        {
            return await _dbContext.EducationLevelCaches.ToListAsync();
        }

        public override async Task<EducationLevelCache?> GetByIdAsync(Guid id)
        {
            return await _dbContext.EducationLevelCaches
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
