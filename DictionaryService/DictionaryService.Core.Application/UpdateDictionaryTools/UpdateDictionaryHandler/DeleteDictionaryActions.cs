using DictionaryService.Core.Domain;
using Common.Repositories;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler
{
    public class DeleteDictionaryActions<TEntity> where TEntity : BaseDictionaryEntity
    {
        public required IBaseWithBaseEntityRepository<TEntity> Repository { get; set; }

        public required Func<bool, TEntity, List<string>, Task<bool>> DeleteEntityAsync { get; init; }
    }
}
