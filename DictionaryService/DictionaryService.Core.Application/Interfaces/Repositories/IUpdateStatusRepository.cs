using DictionaryService.Core.Domain;
using Common.Repositories;
using DictionaryService.Core.Domain.Enum;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IUpdateStatusRepository : IBaseRepository<UpdateStatus> 
    {
        Task<List<UpdateStatus>> GetAllAsync();
        Task<bool> CheckOtherUpdatingAsync();
        Task<UpdateStatus?> GetByDictionaryType(DictionaryType dictionaryType);
    }
}
