using LoggerService.Core.Application.DTOs;
using LoggerService.Core.Domain;
using Common.EasyNetQ.Logger;

namespace LoggerService.Core.Application.Mapper
{
    public static class LogMapper
    {
        public static Log ToLog(this LogDTO log)
        {
            return new()
            {
                LogDateTime = log.LogDateTime.ToUniversalTime(),
                LoggerName = log.LoggerName,
                LogLevel = log.LogLevel,
                LogMessage = log.LogMessage,
            };
        }

        public static LogInfoDTO ToLogInfoDTO(this Log log)
        {
            return new()
            {
                Id = log.Id,
                LogDateTime = log.LogDateTime,
                LoggerName = log.LoggerName,
                LogLevel = log.LogLevel,
                LogMessage = log.LogMessage,
            };
        }
    }
}
