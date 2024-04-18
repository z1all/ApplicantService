using Common.Enums;
using Common.Models;
using Common.DTOs;

namespace DictionaryService.Core.Application.Interfaces.Services
{
    public interface IUpdateDictionaryService
    {
        Task<ExecutionResult> UpdateDictionaryAsync(DictionaryType dictionaryType);
        Task<ExecutionResult> UpdateAllDictionaryAsync();
        Task<ExecutionResult<List<UpdateStatusDTO>>> GetUpdateStatusesAsync();
    }
}
