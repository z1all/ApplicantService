using Common.Models;
using DictionaryService.Core.Application.DTOs;

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
