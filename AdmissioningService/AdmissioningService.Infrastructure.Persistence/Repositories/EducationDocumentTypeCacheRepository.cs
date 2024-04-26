using Microsoft.EntityFrameworkCore;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class EducationDocumentTypeCacheRepository : BaseWithBaseEntityRepository<EducationDocumentTypeCache, AppDbContext>, IEducationDocumentTypeCacheRepository
    {
        public EducationDocumentTypeCacheRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<EducationDocumentTypeCache>> GetAllByNextEducationLevelWithNextLevelId(Guid nextEducationLevelId)
        {
            return await _dbContext.EducationDocumentTypeCaches
                .Include(documentType => documentType.NextEducationLevel)
                .Where(documentType => documentType.NextEducationLevel.Any(level => level.Id == nextEducationLevelId))
                .ToListAsync();
        }

        public override async Task<EducationDocumentTypeCache?> GetByIdAsync(Guid id)
        {
            return await _dbContext.EducationDocumentTypeCaches
                .Include(documentType => documentType.NextEducationLevel)
                .FirstOrDefaultAsync(documentType => documentType.Id == id);
        }
    }
}
