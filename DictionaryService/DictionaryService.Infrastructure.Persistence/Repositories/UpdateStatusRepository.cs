using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using DictionaryService.Core.Domain.Enum;
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

        public async Task<bool> CheckOtherUpdatingAsync()
        {
            bool existOtherUpdating;
            using (IDbContextTransaction scope = await _dbContext.Database.BeginTransactionAsync())
            {
                await _dbContext.Database.ExecuteSqlRawAsync("LOCK TABLE \"Faculties\" IN ACCESS EXCLUSIVE MODE");

                List<UpdateStatusEnum> updatingStatuses = [
                    UpdateStatusEnum.Loading,
                    UpdateStatusEnum.Updating,
                ];

                existOtherUpdating = await _dbContext.UpdateStatuses
                    .AnyAsync(updateStatuses => updatingStatuses.Contains(updateStatuses.Status));

                scope.Commit();
            }
            return existOtherUpdating;
        }

        public async Task<UpdateStatus?> GetByDictionaryType(DictionaryType dictionaryType)
        {
            return await _dbContext.UpdateStatuses
                .FirstOrDefaultAsync(updateStatuses => updateStatuses.DictionaryType == dictionaryType);
        }
    }
}
