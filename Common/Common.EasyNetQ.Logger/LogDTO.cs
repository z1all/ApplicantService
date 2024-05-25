using Microsoft.Extensions.Logging;

namespace Common.EasyNetQ.Logger
{
    public class LogDTO
    {
        public required DateTime LogDateTime { get; set; }
        public required string LogMessage { get; set; }
        public required LogLevel LogLevel { get; set; }
        public required string LoggerName { get; set; }
    }
}
