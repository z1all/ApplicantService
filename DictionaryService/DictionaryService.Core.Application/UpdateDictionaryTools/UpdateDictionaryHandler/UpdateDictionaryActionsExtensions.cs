using DictionaryService.Core.Domain;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler
{
    public static class UpdateDictionaryActionsExtensions
    {
        public static UpdateAndAddDictionaryActions<TEntity, TExternalEntity> ToUpdateAndAddActions<TEntity, TExternalEntity>(
            this UpdateDictionaryActions<TEntity, TExternalEntity> actions) where TEntity : BaseDictionaryEntity
        {
            return new()
            {
                Logger = actions.Logger,
                Repository = actions.Repository,
                CompareKey = actions.CompareKey,
                UpdateEntity = actions.UpdateEntity,
                AddEntity = actions.AddEntity,
                CheckBeforeUpdateEntityAsync = actions.CheckBeforeUpdateEntityAsync,
                CheckBeforeAddEntityAsync = actions.CheckBeforeAddEntityAsync,
            };
        }
        public static DeleteDictionaryActions<TEntity> ToDeleteActions<TEntity, TExternalEntity>(
            this UpdateDictionaryActions<TEntity, TExternalEntity> actions) where TEntity : BaseDictionaryEntity
        {
            return new()
            {
                Logger = actions.Logger,
                Repository = actions.Repository,    
                DeleteEntityAsync = actions.DeleteEntityAsync,
            };
        }
    }
}
