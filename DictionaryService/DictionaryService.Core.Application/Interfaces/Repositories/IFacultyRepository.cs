using DictionaryService.Core.Domain;
using Common.Repositories;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IFacultyRepository : IBaseWithBaseEntityRepository<Faculty> 
    {
        Task<List<Faculty>> GetAllAsync(bool getDeprecated);
    }
}
