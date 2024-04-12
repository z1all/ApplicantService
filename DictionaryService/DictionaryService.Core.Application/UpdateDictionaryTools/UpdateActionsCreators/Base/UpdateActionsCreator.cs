using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler;
using DictionaryService.Core.Domain;
using Common.Models;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators.Base
{
    // Создать базовый абстрактный класс с виртуальными методами которые возвращают делегаты. Также должен быть метод который собирает результаты этих методов и упаковывает в объект
    public abstract class UpdateActionsCreator<TEntity, TExternalEntity>
        where TEntity : BaseDictionaryEntity
        where TExternalEntity : class
    {
        protected abstract bool CompareKey(TEntity entity, TExternalEntity externalEntity);
        protected abstract Task<List<TEntity>> GetEntityAsync();
        protected abstract Task<ExecutionResult<List<TExternalEntity>>> GetExternalEntityAsync();
        protected abstract void UpdateEntity(TEntity entity, TExternalEntity externalEntity);
        protected abstract TEntity AddEntity(TExternalEntity externalEntity);

        protected virtual Task<bool> CheckBeforeUpdateEntityAsync(TEntity entity, TExternalEntity externalEntity, List<string> comments) => Task.FromResult(true);
        protected virtual Task<bool> CheckBeforeAddEntityAsync(TExternalEntity externalEntity, List<string> comments) => Task.FromResult(true);
        protected virtual Task<bool> DeleteEntityAsync(bool deleteRelatedEntities, TEntity entity, List<string> comments) => Task.FromResult(false);

        protected virtual Task BeforeActionsAsync() => Task.CompletedTask;

        public UpdateDictionaryActions<TEntity, TExternalEntity> CreateActions()
        {
            return new()
            {
                BeforeActionsAsync = BeforeActionsAsync,
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
        protected bool SoftDeleteEntityIf<TEntity>(bool deleteRelatedEntities, List<TEntity> relatedEntities, List<string> comments, Func<TEntity, string> onAddErrorMessage)
            where TEntity : BaseDictionaryEntity
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
