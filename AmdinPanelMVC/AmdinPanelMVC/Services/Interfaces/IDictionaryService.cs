using Common.Models.DTOs;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IDictionaryService
    {
        Task<ExecutionResult<List<FacultyDTO>>> GetFacultiesAsync();
    }
}
