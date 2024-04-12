using DictionaryService.Core.Domain;
using Common.Models;
using Common.Repositories;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler
{
    public static class UpdateDictionaryHandler<TEntity, TExternalEntity, TRepository>
        where TEntity : BaseDictionaryEntity
        where TExternalEntity : class
        where TRepository : IBaseRepository<TEntity>
    {
        public static async Task<ExecutionResult> UpdateAsync(
            bool deleteRelatedEntities, TRepository repository,
            UpdateDictionaryActions<TEntity, TExternalEntity> actions)
        {
            await actions.BeforeActionsAsync();

            // Получаем данные в нашей базе данных
            List<TEntity> existEntities = await actions.GetEntityAsync();

            // Получаем данные из внешнего сервиса
            ExecutionResult<List<TExternalEntity>> getExternalEntityResult = await actions.GetExternalEntityAsync();
            if (!getExternalEntityResult.IsSuccess) return getExternalEntityResult;
            List<TExternalEntity> externalEntities = getExternalEntityResult.Result!;

            // Пробуем обновлять и добавлять записи
            ExecutionResult<Dictionary<Guid, TEntity>> updatingAndAddingResult
                = await DoUpdateAndAddEntitiesAsync(repository, actions.ToUpdateAndAddActions(), externalEntities, existEntities);
            if (!updatingAndAddingResult.IsSuccess) return updatingAndAddingResult;
            Dictionary<Guid, TEntity> existEntitiesForRemove = updatingAndAddingResult.Result!;

            // Пробуем удалить записи
            ExecutionResult deletingResult
                = await DoDeleteEntitiesAsync(deleteRelatedEntities, repository, actions.ToDeleteActions(), existEntitiesForRemove);
            if (!deletingResult.IsSuccess) return deletingResult;

            await repository.SaveChangesAsync();

            return new(isSuccess: true);
        }

        private static async Task<ExecutionResult<Dictionary<Guid, TEntity>>> DoUpdateAndAddEntitiesAsync(
            TRepository repository, UpdateAndAddDictionaryActions<TEntity, TExternalEntity> actions,
            List<TExternalEntity> externalEntities, List<TEntity> existEntities)
        {
            // Словарь для хранения тех сущностей, которые существуют только в нашей бд, то есть были удалены во внешнем сервисе
            Dictionary<Guid, TEntity> existEntitiesForRemove = existEntities.ToDictionary(entity => entity.Id);
            bool thereAreNotRelated = false;
            List<string> comments = new();
            foreach (var externalEntity in externalEntities)
            {
                TEntity? existEntity = existEntities.FirstOrDefault(existEntity => actions.CompareKey(existEntity, externalEntity));
                if (existEntity is not null)
                {
                    if (!await actions.CheckBeforeUpdateEntityAsync(existEntity, externalEntity, comments))
                    {
                        thereAreNotRelated = true;
                        continue;
                    }

                    // Обновляем запись...
                    actions.UpdateEntity(existEntity, externalEntity);
                    existEntitiesForRemove.Remove(existEntity.Id);
                }
                else
                {
                    if (!await actions.CheckBeforeAddEntityAsync(externalEntity, comments))
                    {
                        thereAreNotRelated = true;
                        continue;
                    }

                    // Добавляем запись...
                    TEntity newEntity = actions.AddEntity(externalEntity);
                    await repository.AddAsync(newEntity);
                }
            }

            if (thereAreNotRelated)
            {
                return new(
                    keyError: "UpdateOrAddEntityError",
                    error: $"It is not possible to update or add a record because it refers to a non-existent record.\n{string.Join('\n', comments.Take(20))}"
                );
            }

            return new() { Result = existEntitiesForRemove };
        }

        private static async Task<ExecutionResult> DoDeleteEntitiesAsync(
            bool deleteRelatedEntities, TRepository repository,
            DeleteDictionaryActions<TEntity> actions,
            Dictionary<Guid, TEntity> existEntitiesForRemove)
        {
            bool thereAreRelated = false;
            List<string> comments = new();
            foreach (var entityForRemove in existEntitiesForRemove.Values)
            {
                // Если удалена, то пропускаем
                if (entityForRemove.Deprecated) continue;

                TEntity? entity = await repository.GetByIdAsync(entityForRemove.Id);
                if (entity is null)
                {
                    return new(keyError: "UnknownError", error: "Unknown error.");
                }

                // Удаляем запись...
                thereAreRelated = await actions.DeleteEntityAsync(deleteRelatedEntities, entity, comments);
                entity.Deprecated = true;
            }

            if (!deleteRelatedEntities && thereAreRelated)
            {
                return new(
                    keyError: "DeleteEntityError",
                    error: $"It is not possible to delete a record because it is referenced by other records.\n{string.Join('\n', comments.Take(20))}"
                );
            }

            return new(isSuccess: true);
        }
    }
}