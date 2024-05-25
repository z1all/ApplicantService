using Microsoft.Extensions.Logging;
using Common.Repositories;

namespace LoggerService.Core.Domain
{
    public class Log : BaseEntity
    {
        public required DateTime LogDateTime { get; set; }
        public required string LogMessage { get; set; }
        public required LogLevel LogLevel { get; set; }
        public required string LoggerName { get; set; }
        public required string ServiceName { get; set; }
    }
}
