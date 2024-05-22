using Common.Models.DTOs.Admission;
using Common.Models.DTOs.Dictionary;
using Common.Models.Enums;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IAdminService
    {
        Task UpdateAllDictionaryAsync();
        Task UpdateDictionaryAsync(DictionaryType dictionaryType);
        Task<ExecutionResult<List<UpdateStatusDTO>>> GetUpdateStatusesAsync();

        Task<ExecutionResult<List<ManagerProfileDTO>>> GetManagersAsync();
        Task<ExecutionResult> ChangeManagerAsync(Guid managerId, ManagerDTO manager);
        Task<ExecutionResult> AddManagerAsync(ManagerDTO manager, string password);
        Task<ExecutionResult> DeleteManagerAsync(Guid managerId);
    }
}
