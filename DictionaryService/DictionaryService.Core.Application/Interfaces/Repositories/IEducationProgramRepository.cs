using DictionaryService.Core.Domain;
using Common.Repositories;
using DictionaryService.Core.Application.DTOs;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IEducationProgramRepository : IBaseRepository<EducationProgram>
    { 
        Task<List<EducationProgram>> GetAllAsync();
        Task<List<EducationProgram>> GetAllByFacultyIdAsync(Guid facultyId);
        Task<List<EducationProgram>> GetAllByEducationLevelIdAsync(Guid educationLevelId);
        Task<List<EducationProgram>> GetAllByFiltersAsync(EducationProgramFilterDTO filter);
    }
}
