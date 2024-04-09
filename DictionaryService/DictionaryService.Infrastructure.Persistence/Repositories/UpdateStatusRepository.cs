using Microsoft.EntityFrameworkCore;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    public class UpdateStatusRepository : BaseRepository<UpdateStatus, AppDbContext>, IUpdateStatusRepository
    {
        public UpdateStatusRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<List<UpdateStatus>> GetAllAsync()
        {
            return await _dbContext.UpdateStatuses.ToListAsync();
        }
    }
}
