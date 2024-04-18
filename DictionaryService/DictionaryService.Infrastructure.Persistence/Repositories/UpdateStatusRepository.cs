using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Enums;
using Common.Repositories;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    public class UpdateStatusRepository : BaseRepository<UpdateStatus, UpdateStatusDbContext>, IUpdateStatusRepository
    {
        public UpdateStatusRepository(UpdateStatusDbContext appDbContext) : base(appDbContext) { }

        public async Task<List<UpdateStatus>> GetAllAsync()
        {
            return await _dbContext.UpdateStatuses.ToListAsync();
        }

        public async Task<UpdateStatus?> GetByDictionaryTypeAsync(DictionaryType dictionaryType)
        {
            return await _dbContext.UpdateStatuses
                .FirstOrDefaultAsync(updateStatuses => updateStatuses.DictionaryType == dictionaryType);
        }

        public async Task<bool> TryBeganUpdatingForDictionaryAsync(DictionaryType dictionaryType)
        {
            return await CheckOtherUpdatingAsync(async () =>
            {
                UpdateStatus? updateStatus = await _dbContext.UpdateStatuses
                    .FirstAsync(updateStatus => updateStatus.DictionaryType == dictionaryType);

                updateStatus.Status = UpdateStatusEnum.Wait;
                updateStatus.Comments = null;

                await _dbContext.SaveChangesAsync();
            });
        }

        public async Task<bool> TryBeganUpdatingForAllDictionaryAsync()
        {
            return await CheckOtherUpdatingAsync(async () =>
            {
                List<UpdateStatus> updateStatuses = await _dbContext.UpdateStatuses.ToListAsync();

                updateStatuses.ForEach(updateStatus => {
                    updateStatus.Status = UpdateStatusEnum.Wait;
                    updateStatus.Comments = null;
                });

                await _dbContext.SaveChangesAsync();
            });
        }

        private async Task<bool> CheckOtherUpdatingAsync(Func<Task> actionAsync)
        {
            bool existOtherUpdating = false;

            await DoExclusiveTransactionAsync(async () =>
            {
                List<UpdateStatusEnum> updatingStatuses = [
                   UpdateStatusEnum.Loading,
                    UpdateStatusEnum.Updating,
                    UpdateStatusEnum.Wait,
                ];

                existOtherUpdating = await _dbContext.UpdateStatuses
                    .AnyAsync(updateStatuses => updatingStatuses.Contains(updateStatuses.Status));

                if (!existOtherUpdating)
                {
                    await actionAsync();
                }
            });

            return existOtherUpdating;
        }

        private async Task DoExclusiveTransactionAsync(Func<Task> actionAsync)
        {
            using (IDbContextTransaction scope = await _dbContext.Database.BeginTransactionAsync())
            {
                await _dbContext.Database.ExecuteSqlRawAsync("LOCK TABLE \"Faculties\" IN ACCESS EXCLUSIVE MODE");

                await actionAsync();

                scope.Commit();
            }
        }
    }
}
