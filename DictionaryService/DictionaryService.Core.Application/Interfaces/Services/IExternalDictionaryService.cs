using Common.Models.Models;
using DictionaryService.Core.Application.DTOs;

namespace DictionaryService.Core.Application.Interfaces.Services
{
    public interface IExternalDictionaryService
    {
        Task<ExecutionResult<List<EducationLevelExternalDTO>>> GetEducationLevelsAsync();
        Task<ExecutionResult<List<EducationDocumentTypeExternalDTO>>> GetEducationDocumentTypesAsync();
        Task<ExecutionResult<List<FacultyExternalDTO>>> GetFacultiesAsync();
        Task<ExecutionResult<List<EducationProgramExternalDTO>>> GetEducationProgramAsync();
    }
}
