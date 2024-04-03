using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using ApplicantService.Infrastructure.Persistence.Contexts;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class EducationDocumentTypeCacheRepository : BaseRepository<EducationDocumentTypeCache>, IEducationDocumentTypeCacheRepository
    {
        public EducationDocumentTypeCacheRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
