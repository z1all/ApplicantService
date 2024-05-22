using DictionaryService.Core.Domain;
using Common.Repositories;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IEducationDocumentTypeRepository : IBaseWithBaseEntityRepository<EducationDocumentType> 
    {
        Task<List<EducationDocumentType>> GetAllByNextEducationLevelIdAsync(Guid educationLevelId);
        Task<List<EducationDocumentType>> GetByCurrentEducationLevelIdAsync(Guid educationLevelId);
        Task<List<EducationDocumentType>> GetAllAsync(bool getDeprecated);
        Task<EducationDocumentType?> GetByIdAsync(Guid id, bool getDeprecated);
    }
}
