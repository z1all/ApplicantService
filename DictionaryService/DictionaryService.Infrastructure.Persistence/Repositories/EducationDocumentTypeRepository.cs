using Microsoft.EntityFrameworkCore;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    public class EducationDocumentTypeRepository : BaseRepository<EducationDocumentType, AppDbContext>, IEducationDocumentTypeRepository
    {
        public EducationDocumentTypeRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<List<EducationDocumentType>> GetAllByNextEducationLevelIdAsync(Guid educationLevelId)
        {
            return await _dbContext.EducationDocumentTypes
                .Where(educationDocumentTypes =>
                    educationDocumentTypes.NextEducationLevels.Any(educationLevels => educationLevels.Id == educationLevelId)
                ).ToListAsync();
        }

        public async Task<EducationDocumentType> GetByCurrentEducationLevelIdProgram(Guid educationLevelId)
        {
            return await _dbContext.EducationDocumentTypes
                .FirstAsync(educationDocumentTypes => educationDocumentTypes.EducationLevelId == educationLevelId);
        }
    }
}