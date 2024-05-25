using LoggerService.Core.Application.Interfaces;
using LoggerService.Core.Application.Mapper;
using Common.EasyNetQ.Logger;
using Common.EasyNetQ.Logger.Receiver.Interfaces;
using Common.Models.Models;
using LoggerService.Core.Application.DTOs;
using LoggerService.Core.Domain;

namespace LoggerService.Core.Application.Services
{
    public class LogService : ILogService, IReceiverService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<ExecutionResult<LogPagedDTO>> GetLogsAsync(LogFilterDTO logFilter)
        {
            if (logFilter.Page < 1)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "InvalidPageError", error: "Number of page can't be less than 1.");
            }

            int countLogs = await _logRepository.GetCountByFiler(logFilter);
            countLogs = countLogs == 0 ? 1 : countLogs;

            int countPage = (countLogs + logFilter.Size - 1) / logFilter.Size;
            if (logFilter.Page > countPage)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "InvalidPageError", error: $"Number of page can be from 1 to {countPage}.");
            }

            List<Log> logs = await _logRepository.GetAllByFiler(logFilter);
            return new()
            {
                Result = new()
                {
                    Logs = logs.Select(log => log.ToLogInfoDTO()).ToList(),
                    Pagination = new()
                    {
                        Count = countPage,
                        Current = logFilter.Page,
                        Size = logFilter.Size,
                    },
                },
            };
        }

        public async Task Processing(LogDTO log)
        {
            await _logRepository.AddAsync(log.ToLog());
        }
    }
}
