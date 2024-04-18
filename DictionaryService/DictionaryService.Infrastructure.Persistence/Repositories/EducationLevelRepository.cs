using Microsoft.EntityFrameworkCore;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    internal class EducationLevelRepository : BaseRepository<EducationLevel, AppDbContext>, IEducationLevelRepository
    {
        public EducationLevelRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<bool> AnyByExternalIdAsync(int externalId)
        {
            return await _dbContext.EducationLevels
                .AnyAsync(educationLevel => educationLevel.ExternalId == externalId);
        }

        public async Task<List<EducationLevel>> GetAllAsync(bool getDeprecated)
        {
            return await _dbContext.EducationLevels
                .Where(educationLevels => getDeprecated ? true : !educationLevels.Deprecated)
                .ToListAsync();
        }

        public EducationLevel GetByExternalId(int externalId)
        {
            return _dbContext.EducationLevels
                .First(educationLevel => educationLevel.ExternalId == externalId);
        }

        public override async Task<EducationLevel?> GetByIdAsync(Guid id)
        {
            return await _dbContext.EducationLevels
                .FirstOrDefaultAsync(educationLevel => educationLevel.Id == id);
        }
    }
}
