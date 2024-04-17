using Common.Models;
using Common.DTOs;

namespace DictionaryService.Core.Application.Interfaces.Services
{
    public interface IDictionaryInfoService
    {
        Task<ExecutionResult<ProgramPagedDTO>> GetProgramsAsync(EducationProgramFilterDTO filter);
        Task<ExecutionResult<List<EducationLevelDTO>>> GetEducationLevelsAsync();
        Task<ExecutionResult<List<EducationDocumentTypeDTO>>> GetDocumentTypesAsync();
        Task<ExecutionResult<List<FacultyDTO>>> GetFacultiesAsync();
    }
}
