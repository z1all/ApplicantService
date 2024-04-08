using Common.Repositories;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    public class UpdateStatusRepository : BaseRepository<UpdateStatus, AppDbContext>, IUpdateStatusRepository
    {
        public UpdateStatusRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
