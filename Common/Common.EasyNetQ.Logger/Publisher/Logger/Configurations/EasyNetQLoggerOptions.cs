using Microsoft.Extensions.Logging;

namespace Common.EasyNetQ.Logger.Configurations
{
    public class EasyNetQLoggerOptions
    {
        public required LogLevel DefaultLogLevel { get; set; }
    }
}
