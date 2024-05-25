using Microsoft.Extensions.Logging;
using LoggerService.Core.Application.Enums;

namespace LoggerService.Core.Application.DTOs
{
    public class LogFilterDTO
    {
        public string? ServiceName { get; set; }
        public string? LoggerName { get; set; }
        public LogLevel? LogLevel { get; set; }
        public LogSortType SortType { get; set; }

        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
