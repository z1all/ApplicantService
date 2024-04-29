using Microsoft.EntityFrameworkCore;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class EducationProgramCacheRepository : BaseWithBaseEntityRepository<EducationProgramCache, AppDbContext>, IEducationProgramCacheRepository
    {
        public EducationProgramCacheRepository(AppDbContext dbContext) : base(dbContext) { }

        public Task<EducationProgramCache?> GetByIdWithLevelAsync(Guid programId)
        {
            return _dbContext.EducationProgramCaches
                .Include(program => program.EducationLevel)
                .FirstOrDefaultAsync(program => program.Id == programId);
        }

        public Task<EducationProgramCache?> GetByIdWithoutLevelAsync(Guid programId)
        {
            return _dbContext.EducationProgramCaches
                .FirstOrDefaultAsync(program => program.Id == programId);
        }
    }
}
