using Microsoft.Extensions.Logging;
using Common.EasyNetQ.Logger.Configurations;
using EasyNetQ;

namespace Common.EasyNetQ.Logger
{
    public class EasyNetQLogger : ILogger
    {
        private readonly IBus _bus;
        private readonly EasyNetQLoggerOptions _easyNetQLoggerOptions;

        public EasyNetQLogger(IBus bus, EasyNetQLoggerOptions easyNetQLoggerOptions)
        {
            _bus = bus;
            _easyNetQLoggerOptions = easyNetQLoggerOptions;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _easyNetQLoggerOptions.DefaultLogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    _bus.PubSub.Publish<LogDTO>(new()
                    {
                        LogMessage = formatter(state, exception),
                        LogDateTime = DateTime.Now,
                        LogLevel = logLevel,
                        LoggerName = eventId.Name ?? "",
                    });
                }
                catch (Exception ex)
                {

                }
            });
        }
    }
}
