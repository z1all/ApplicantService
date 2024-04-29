using DictionaryService.Core.Domain;
using Common.Repositories;
using Common.Models.DTOs;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IEducationProgramRepository : IBaseWithBaseEntityRepository<EducationProgram>
    { 
        Task<List<EducationProgram>> GetAllAsync();
        Task<List<EducationProgram>> GetAllByFacultyIdAsync(Guid facultyId);
        Task<List<EducationProgram>> GetAllByEducationLevelIdAsync(Guid educationLevelId);
        Task<List<EducationProgram>> GetAllByFiltersAsync(EducationProgramFilterDTO filter, bool getDeprecated);
        Task<int> GetAllCountAsync(EducationProgramFilterDTO filter, bool getDeprecated);
        Task<EducationProgram?> GetByIdWithFacultyAndLevelAsync(Guid programId);
    }
}
