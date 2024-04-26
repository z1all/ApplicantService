using Microsoft.EntityFrameworkCore;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    public class EducationDocumentTypeRepository : BaseWithBaseEntityRepository<EducationDocumentType, AppDbContext>, IEducationDocumentTypeRepository
    {
        public EducationDocumentTypeRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<List<EducationDocumentType>> GetAllAsync(bool getDeprecated)
        {
            return await _dbContext.EducationDocumentTypes
                .Include(documentTypes => documentTypes.EducationLevel)
                .Include(documentTypes => documentTypes.NextEducationLevels
                                            .Where(educationLevel => getDeprecated ? true : !educationLevel.Deprecated))
                .Where(documentTypes => getDeprecated ? true : !documentTypes.Deprecated && !documentTypes.EducationLevel!.Deprecated)
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
                .Include(documentTypes => documentTypes.EducationLevel)
                .Include(documentTypes => documentTypes.NextEducationLevels)
                .FirstOrDefaultAsync(Faculty => Faculty.Id == id);
        }

        public async Task<EducationDocumentType?> GetByIdAsync(Guid id, bool getDeprecated)
        {
            return await _dbContext.EducationDocumentTypes
                .Include(documentTypes => documentTypes.EducationLevel)
                .Include(documentTypes => documentTypes.NextEducationLevels
                                            .Where(educationLevel => getDeprecated ? true : !educationLevel.Deprecated))
                .FirstOrDefaultAsync(documentTypes => documentTypes.Id == id && 
                                                     (getDeprecated ? true : !documentTypes.Deprecated && !documentTypes.EducationLevel!.Deprecated));
        }
    }
}