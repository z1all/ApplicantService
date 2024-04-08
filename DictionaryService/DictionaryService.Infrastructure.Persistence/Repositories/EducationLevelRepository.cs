using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    internal class EducationLevelRepository : BaseRepository<EducationLevel, AppDbContext>, IEducationLevelRepository
    {
        public EducationLevelRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
