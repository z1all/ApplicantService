using LoggerService.Core.Application.DTOs;
using Common.Models.Models;

namespace LoggerService.Core.Application.Interfaces
{
    public interface ILogService
    {
        Task<ExecutionResult<LogPagedDTO>> GetLogsAsync(LogFilterDTO logFilter);
    }
}
