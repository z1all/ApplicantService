using DictionaryService.Core.Domain;
using Common.Repositories;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IEducationLevelRepository : IBaseWithBaseEntityRepository<EducationLevel> 
    {
        Task<List<EducationLevel>> GetAllAsync(bool getDeprecated);
        EducationLevel GetByExternalId(int externalId);
        Task<bool> AnyByExternalIdAsync(int externalId);
    }
}
