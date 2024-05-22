using Common.Models.DTOs.Dictionary;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IDictionaryService
    {
        Task<ExecutionResult<List<FacultyDTO>>> GetFacultiesAsync();
        Task<ExecutionResult<List<EducationDocumentTypeDTO>>> GetEducationDocumentTypesAsync();
    }
}
