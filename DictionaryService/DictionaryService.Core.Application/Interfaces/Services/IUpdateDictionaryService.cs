using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Domain.Enum;
using Common.Models;

namespace DictionaryService.Core.Application.Interfaces.Services
{
    public interface IUpdateDictionaryService
    {
        Task<ExecutionResult> UpdateDictionaryAsync(DictionaryType dictionaryType);
        Task<ExecutionResult> UpdateAllDictionaryAsync();
        Task<ExecutionResult<List<UpdateStatusDTO>>> GetUpdateStatusesAsync();
    }
}
