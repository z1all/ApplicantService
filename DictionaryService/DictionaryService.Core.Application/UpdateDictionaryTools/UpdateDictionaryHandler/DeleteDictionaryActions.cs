namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler
{
    public class DeleteDictionaryActions<TEntity>
    {
        public required Func<bool, TEntity, List<string>, Task<bool>> DeleteEntityAsync { get; init; }
    }
}
