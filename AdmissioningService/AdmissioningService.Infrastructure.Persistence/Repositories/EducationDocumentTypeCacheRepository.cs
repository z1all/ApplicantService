using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class EducationDocumentTypeCacheRepository : BaseRepository<EducationDocumentTypeCache, AppDbContext>, IEducationDocumentTypeCacheRepository
    {
        public EducationDocumentTypeCacheRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<EducationDocumentTypeCache>> GetAllAsync()
        {
            return await _dbContext.EducationDocumentTypeCaches.ToListAsync();
        }

        public override async Task<EducationDocumentTypeCache?> GetByIdAsync(Guid id)
        {
            return await _dbContext.EducationDocumentTypeCaches
                .FirstOrDefaultAsync(documentType => documentType.Id == id);
        }
    }
}
