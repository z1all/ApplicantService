using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    public class EducationDocumentTypeRepository : BaseRepository<EducationDocumentType, AppDbContext>, IEducationDocumentTypeRepository
    {
        public EducationDocumentTypeRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
