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

        public override async Task<EducationDocumentTypeCache?> GetByIdAsync(Guid id)
        {
            return await _dbContext.EducationDocumentTypeCaches
                .Include(documentType => documentType.NextEducationLevel)
                .FirstOrDefaultAsync(documentType => documentType.Id == id);
        }
    }
}
