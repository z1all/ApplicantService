using DictionaryService.Core.Domain;
using DictionaryService.Core.Domain.Enum;
using Common.Repositories;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IUpdateStatusRepository : IBaseRepository<UpdateStatus> 
    {
        Task<List<UpdateStatus>> GetAllAsync();
        Task<bool> CheckOtherUpdatingAsync();
        Task<UpdateStatus?> GetByDictionaryType(DictionaryType dictionaryType);
    }
}
