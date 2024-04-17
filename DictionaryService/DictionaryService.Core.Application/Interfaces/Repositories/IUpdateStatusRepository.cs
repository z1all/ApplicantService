using DictionaryService.Core.Domain;
using Common.Enums;
using Common.Repositories;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IUpdateStatusRepository : IBaseRepository<UpdateStatus> 
    {
        Task<List<UpdateStatus>> GetAllAsync();
        Task<bool> TryBeganUpdatingForDictionaryAsync(DictionaryType dictionaryType);
        Task<bool> TryBeganUpdatingForAllDictionaryAsync();
        Task<UpdateStatus?> GetByDictionaryTypeAsync(DictionaryType dictionaryType);
    }
}
