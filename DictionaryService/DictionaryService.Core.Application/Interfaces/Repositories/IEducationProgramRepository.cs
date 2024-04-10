using DictionaryService.Core.Domain;
using Common.Repositories;

namespace DictionaryService.Core.Application.Interfaces.Repositories
{
    public interface IEducationProgramRepository : IBaseRepository<EducationProgram>
    { 
        Task<List<EducationProgram>> GetAllByFacultyIdAsync(Guid facultyId);
    }
}
