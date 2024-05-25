using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Common.EasyNetQ.Logger.Configurations;
using EasyNetQ;

namespace Common.EasyNetQ.Logger
{
    public class EasyNetQLoggerProvider : ILoggerProvider
    {
        private readonly string _serviceName;
        private readonly IServiceProvider _serviceProvider;

        public EasyNetQLoggerProvider(string serviceName, IServiceProvider serviceProvider)
        {
            _serviceName = serviceName;
            _serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var bus = _serviceProvider.GetRequiredService<IBus>();
            var options = _serviceProvider.GetRequiredService<IOptions<EasyNetQLoggerOptions>>();

            return new EasyNetQLogger(_serviceName, bus, options.Value);
        }

        public void Dispose() { }
    }
}
