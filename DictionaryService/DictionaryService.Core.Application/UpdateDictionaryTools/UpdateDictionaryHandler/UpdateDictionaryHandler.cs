using DictionaryService.Core.Domain;
using Common.Models;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler
{
    public static class UpdateDictionaryHandler<TEntity, TExternalEntity>
        where TEntity : BaseDictionaryEntity
        where TExternalEntity : class
    {
        public static async Task<ExecutionResult<List<TEntity>>> UpdateAsync(
            bool deleteRelatedEntities,
            UpdateDictionaryActions<TEntity, TExternalEntity> actions)
        {
            List<TEntity> changedEntities = new List<TEntity>();

            await actions.BeforeActionsAsync();

            // Получаем данные в нашей базе данных
            List<TEntity> existEntities = await actions.GetEntityAsync();

            // Получаем данные из внешнего сервиса
            ExecutionResult<List<TExternalEntity>> getExternalEntityResult = await actions.GetExternalEntityAsync();
            if (!getExternalEntityResult.IsSuccess) 
            {
                await actions.AfterLoadingErrorAsync(getExternalEntityResult.Errors.Values.FirstOrDefault()?[0] ?? "Unknow error");
                return new() { Errors = getExternalEntityResult.Errors }; 
            } 
            List<TExternalEntity> externalEntities = getExternalEntityResult.Result!;

            await actions.BeforeUpdatingAsync();

            // Пробуем обновлять и добавлять записи
            ExecutionResult<Dictionary<Guid, TEntity>> updatingAndAddingResult
                = await DoUpdateAndAddEntitiesAsync(actions.ToUpdateAndAddActions(), externalEntities, existEntities, changedEntities);
            if (!updatingAndAddingResult.IsSuccess) 
            {
                await actions.AfterUpdatingErrorAsync(updatingAndAddingResult.Errors.Values.FirstOrDefault()?[0] ?? "Unknow error");
                return new() { Errors = updatingAndAddingResult.Errors }; 
            }
            Dictionary<Guid, TEntity> existEntitiesForRemove = updatingAndAddingResult.Result!;

            // Пробуем удалить записи
            ExecutionResult deletingResult = await DoDeleteEntitiesAsync(deleteRelatedEntities, actions.ToDeleteActions(), existEntitiesForRemove);
            if (!deletingResult.IsSuccess)
            {
                await actions.AfterUpdatingErrorAsync(updatingAndAddingResult.Errors.Values.FirstOrDefault()?[0] ?? "Unknow error");
                return new() { Errors = deletingResult.Errors }; 
            }
            changedEntities.AddRange(existEntitiesForRemove.Values);

            await actions.Repository.SaveChangesAsync();

            await actions.AfterUpdateAsync();

            return new() { Result = changedEntities };
        }

        private static async Task<ExecutionResult<Dictionary<Guid, TEntity>>> DoUpdateAndAddEntitiesAsync(
            UpdateAndAddDictionaryActions<TEntity, TExternalEntity> actions,
            List<TExternalEntity> externalEntities, List<TEntity> existEntities, List<TEntity> changedEntities)
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
                    bool changed = actions.UpdateEntity(existEntity, externalEntity);
                    if(changed) changedEntities.Add(existEntity);
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
                    await actions.Repository.AddAsync(newEntity);
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
            bool deleteRelatedEntities,
            DeleteDictionaryActions<TEntity> actions,
            Dictionary<Guid, TEntity> existEntitiesForRemove)
        {
            bool thereAreRelated = false;
            List<string> comments = new();
            foreach (var entityForRemove in existEntitiesForRemove.Values)
            {
                // Если удалена, то пропускаем
                if (entityForRemove.Deprecated) continue;

                TEntity? entity = await actions.Repository.GetByIdAsync(entityForRemove.Id);
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