using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Application.Interfaces.Repositories;
using Common.Repositories;
using Common.Models.Models;
using Common.Models.Enums;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators.Base
{
    public abstract class UpdateActionsCreator<TEntity, TExternalEntity>
        where TEntity : BaseDictionaryEntity
        where TExternalEntity : class
    {
        protected abstract UpdateStatus UpdateStatusCache { get; }
        protected abstract IUpdateStatusRepository UpdateStatusRepository { get; }
        protected abstract IBaseWithBaseEntityRepository<TEntity> Repository { get; }

        protected abstract bool CompareKey(TEntity entity, TExternalEntity externalEntity);
        protected abstract Task<List<TEntity>> GetEntityAsync();
        protected abstract Task<ExecutionResult<List<TExternalEntity>>> GetExternalEntityAsync();
        protected abstract bool UpdateEntity(TEntity entity, TExternalEntity externalEntity);
        protected abstract TEntity AddEntity(TExternalEntity externalEntity);

        protected virtual Task<bool> CheckBeforeUpdateEntityAsync(TEntity entity, TExternalEntity externalEntity, List<string> comments) => Task.FromResult(true);
        protected virtual Task<bool> CheckBeforeAddEntityAsync(TExternalEntity externalEntity, List<string> comments) => Task.FromResult(true);
        protected virtual Task<bool> DeleteEntityAsync(bool deleteRelatedEntities, TEntity entity, List<string> comments) => Task.FromResult(false);

        protected virtual async Task BeforeActionsAsync()
        {
            UpdateStatusCache.Status = UpdateStatusEnum.Loading;
            UpdateStatusCache.Comments = null;
            await UpdateStatusRepository.UpdateAsync(UpdateStatusCache);
        }

        protected virtual async Task AfterLoadingErrorAsync(string comments)
        {
            UpdateStatusCache.Status = UpdateStatusEnum.ErrorInLoading;
            UpdateStatusCache.Comments = comments;
            await UpdateStatusRepository.UpdateAsync(UpdateStatusCache);
        }
        protected virtual async Task BeforeUpdatingAsync()
        {
            UpdateStatusCache.Status = UpdateStatusEnum.Updating;
            UpdateStatusCache.Comments = null;
            await UpdateStatusRepository.UpdateAsync(UpdateStatusCache);
        }
        protected virtual async Task AfterUpdatingErrorAsync(string comments)
        {
            UpdateStatusCache.Status = UpdateStatusEnum.ErrorInUpdating;
            UpdateStatusCache.Comments = comments;
            await UpdateStatusRepository.UpdateAsync(UpdateStatusCache);
        }
        protected virtual async Task AfterUpdateAsync()
        {
            UpdateStatusCache.Status = UpdateStatusEnum.Updated;
            UpdateStatusCache.Comments = null;
            UpdateStatusCache.LastUpdate = DateTime.UtcNow;
            await UpdateStatusRepository.UpdateAsync(UpdateStatusCache);
        }

        public UpdateDictionaryActions<TEntity, TExternalEntity> CreateActions()
        {
            return new()
            {
                Repository = Repository,

                BeforeActionsAsync = BeforeActionsAsync,
                AfterLoadingErrorAsync = AfterLoadingErrorAsync,
                BeforeUpdatingAsync = BeforeUpdatingAsync,
                AfterUpdatingErrorAsync = AfterUpdatingErrorAsync,
                AfterUpdateAsync = AfterUpdateAsync,

                CompareKey = CompareKey,
                GetEntityAsync = GetEntityAsync,
                GetExternalEntityAsync = GetExternalEntityAsync,
                CheckBeforeUpdateEntityAsync = CheckBeforeUpdateEntityAsync,
                UpdateEntity = UpdateEntity,
                CheckBeforeAddEntityAsync = CheckBeforeAddEntityAsync,
                AddEntity = AddEntity,
                DeleteEntityAsync = DeleteEntityAsync,
            };
        }
        protected bool SoftDeleteEntityIf<TRelatedEntity>(bool deleteRelatedEntities, List<TRelatedEntity> relatedEntities, List<string> comments, Func<TRelatedEntity, string> onAddErrorMessage)
            where TRelatedEntity : BaseDictionaryEntity
        {
            bool thereAreRelated = false;

            foreach (var relatedEntity in relatedEntities)
            {
                if (!deleteRelatedEntities && !relatedEntity.Deprecated)
                {
                    thereAreRelated = true;
                    comments.Add(onAddErrorMessage(relatedEntity));
                    continue;
                }

                relatedEntity.Deprecated = true;
            }

            return thereAreRelated;
        }
    }
}
