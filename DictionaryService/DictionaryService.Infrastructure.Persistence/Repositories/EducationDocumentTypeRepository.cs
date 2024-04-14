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

        public async Task<List<EducationDocumentType>> GetAllAsync()
        {
            return await _dbContext.EducationDocumentTypes
                .Include(educationDocumentTypes => educationDocumentTypes.EducationLevel)
                .Include(educationDocumentTypes => educationDocumentTypes.NextEducationLevels)
                .ToListAsync();
        }

        public async Task<List<EducationDocumentType>> GetAllByNextEducationLevelIdAsync(Guid educationLevelId)
        {
            return await _dbContext.EducationDocumentTypes
                .Where(educationDocumentTypes =>
                    educationDocumentTypes.NextEducationLevels.Any(educationLevels => educationLevels.Id == educationLevelId)
                ).ToListAsync();
        }

        public async Task<List<EducationDocumentType>> GetByCurrentEducationLevelIdAsync(Guid educationLevelId)
        {
            return await _dbContext.EducationDocumentTypes
                .Where(educationDocumentTypes => educationDocumentTypes.EducationLevelId == educationLevelId)
                .ToListAsync();
        }

        public override async Task<EducationDocumentType?> GetByIdAsync(Guid id)
        {
            return await _dbContext.EducationDocumentTypes
                .FirstOrDefaultAsync(Faculty => Faculty.Id == id);
        }
    }
}