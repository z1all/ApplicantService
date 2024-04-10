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

        public async Task<List<EducationLevel>> GetAllAsync()
        {
            return await _dbContext.EducationLevels.ToListAsync();
        }

        public override async Task<EducationLevel?> GetByIdAsync(Guid id)
        {
            return await _dbContext.EducationLevels
                .FirstOrDefaultAsync(educationLevel => educationLevel.Id == id);
        }
    }
}
