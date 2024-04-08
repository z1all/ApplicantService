using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    public class FacultyRepository : BaseRepository<Faculty, AppDbContext>, IFacultyRepository
    {
        public FacultyRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
