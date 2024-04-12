namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler
{
    public static class UpdateDictionaryActionsExtensions
    {
        public static UpdateAndAddDictionaryActions<TEntity, TExternalEntity> ToUpdateAndAddActions<TEntity, TExternalEntity>(
            this UpdateDictionaryActions<TEntity, TExternalEntity> actions)
        {
            return new()
            {
                CompareKey = actions.CompareKey,
                UpdateEntity = actions.UpdateEntity,
                AddEntity = actions.AddEntity,
                CheckBeforeUpdateEntityAsync = actions.CheckBeforeUpdateEntityAsync,
                CheckBeforeAddEntityAsync = actions.CheckBeforeAddEntityAsync,
            };
        }
        public static DeleteDictionaryActions<TEntity> ToDeleteActions<TEntity, TExternalEntity>(
            this UpdateDictionaryActions<TEntity, TExternalEntity> actions)
        {
            return new()
            {
                DeleteEntityAsync = actions.DeleteEntityAsync,
            };
        }
    }
}
