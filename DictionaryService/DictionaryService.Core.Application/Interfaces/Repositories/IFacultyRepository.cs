using DictionaryService.Core.Domain;
using Common.Repositories;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IFacultyRepository : IBaseRepository<Faculty> 
    {
        Task<List<Faculty>> GetAllAsync(bool getDeprecated);
    }
}
