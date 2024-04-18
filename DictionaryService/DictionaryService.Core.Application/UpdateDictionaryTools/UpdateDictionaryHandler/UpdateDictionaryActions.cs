using DictionaryService.Core.Domain;
using Common.Repositories;
using Common.Models.Models;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateDictionaryHandler
{
    public class UpdateDictionaryActions<TEntity, TExternalEntity> where TEntity : BaseDictionaryEntity
    {
        public required IBaseRepository<TEntity> Repository { get; set; }

        public required Func<Task> BeforeActionsAsync { get; init; }
        public required Func<string, Task> AfterLoadingErrorAsync { get; init; }
        public required Func<Task> BeforeUpdatingAsync { get; init; }
        public required Func<string, Task> AfterUpdatingErrorAsync { get; init; }
        public required Func<Task> AfterUpdateAsync { get; init; }

        public required Func<TEntity, TExternalEntity, bool> CompareKey { get; init; }
        public required Func<Task<List<TEntity>>> GetEntityAsync { get; init; }
        public required Func<Task<ExecutionResult<List<TExternalEntity>>>> GetExternalEntityAsync { get; init; }
        public required Func<TEntity, TExternalEntity, List<string>, Task<bool>> CheckBeforeUpdateEntityAsync { get; init; }
        public required Func<TEntity, TExternalEntity, bool> UpdateEntity { get; init; }
        public required Func<TExternalEntity, List<string>, Task<bool>> CheckBeforeAddEntityAsync { get; init; }
        public required Func<TExternalEntity, TEntity> AddEntity { get; init; }
        public required Func<bool, TEntity, List<string>, Task<bool>> DeleteEntityAsync { get; init; }
    }
}
