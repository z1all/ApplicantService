using Common.Models.DTOs;
using Common.Models.Models;

namespace DictionaryService.Core.Application.Interfaces.Services
{
    public interface IDictionaryInfoService
    {
        Task<ExecutionResult<ProgramPagedDTO>> GetProgramsAsync(EducationProgramFilterDTO filter);
        Task<ExecutionResult<EducationProgramDTO>> GetEducationProgramByIdAsync(Guid programId);
        Task<ExecutionResult<List<EducationLevelDTO>>> GetEducationLevelsAsync();
        Task<ExecutionResult<List<EducationDocumentTypeDTO>>> GetDocumentTypesAsync();
        Task<ExecutionResult<EducationDocumentTypeDTO>> GetDocumentTypeByIdAsync(Guid documentTypeId);
        Task<ExecutionResult<List<FacultyDTO>>> GetFacultiesAsync();
        Task<ExecutionResult<FacultyDTO>> GetFacultyAsync(Guid facultyId);
    }
}
