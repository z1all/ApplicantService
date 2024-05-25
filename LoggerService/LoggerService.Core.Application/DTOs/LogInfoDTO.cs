using Microsoft.Extensions.Logging;

namespace LoggerService.Core.Application.DTOs
{
    public class LogInfoDTO
    {
        public required Guid Id{ get; set; }
        public required DateTime LogDateTime { get; set; }
        public required string LogMessage { get; set; }
        public required LogLevel LogLevel { get; set; }
        public required string LoggerName { get; set; }
    }
}
