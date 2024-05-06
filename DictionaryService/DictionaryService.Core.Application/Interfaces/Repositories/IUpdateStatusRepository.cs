using DictionaryService.Core.Domain;
using Common.Repositories;
using Common.Models.Enums;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IUpdateStatusRepository : IBaseWithBaseEntityRepository<UpdateStatus> 
    {
        Task<List<UpdateStatus>> GetAllAsync();
        Task<bool> TryBeganUpdatingForDictionaryAsync(DictionaryType dictionaryType);
        Task<bool> TryBeganUpdatingForAllDictionaryAsync();
        Task<UpdateStatus?> GetByDictionaryTypeAsync(DictionaryType dictionaryType);
    }
}
