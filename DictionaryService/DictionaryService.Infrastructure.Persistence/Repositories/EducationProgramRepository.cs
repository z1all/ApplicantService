using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    public class EducationProgramRepository : BaseRepository<EducationProgram, AppDbContext>, IEducationProgramRepository
    {
        public EducationProgramRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
