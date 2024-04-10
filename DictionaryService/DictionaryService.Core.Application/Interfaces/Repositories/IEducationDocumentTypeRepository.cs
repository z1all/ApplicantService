using DictionaryService.Core.Domain;
using Common.Repositories;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IEducationDocumentTypeRepository : IBaseRepository<EducationDocumentType> 
    {
        Task<List<EducationDocumentType>> GetAllByNextEducationLevelIdAsync(Guid educationLevelId);
        Task<EducationDocumentType> GetByCurrentEducationLevelIdProgram(Guid educationLevelId);
    }
}
