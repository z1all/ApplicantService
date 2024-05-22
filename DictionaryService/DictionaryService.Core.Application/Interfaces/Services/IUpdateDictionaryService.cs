using Common.Models.Models;
using Common.Models.Enums;
using Common.Models.DTOs.Dictionary;

namespace DictionaryService.Core.Application.Interfaces.Services
{
    public interface IUpdateDictionaryService
    {
        Task<ExecutionResult> UpdateDictionaryAsync(DictionaryType dictionaryType);
        Task<ExecutionResult> UpdateAllDictionaryAsync();
        Task<ExecutionResult<List<UpdateStatusDTO>>> GetUpdateStatusesAsync();
    }
}
