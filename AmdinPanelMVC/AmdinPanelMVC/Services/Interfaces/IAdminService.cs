using Common.Models.DTOs;
using Common.Models.Enums;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IAdminService
    {
        Task UpdateAllDictionaryAsync();
        Task UpdateDictionaryAsync(DictionaryType dictionaryType);
        Task<ExecutionResult<List<UpdateStatusDTO>>> GetUpdateStatusesAsync();
    }
}
