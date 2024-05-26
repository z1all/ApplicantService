using Microsoft.Extensions.Logging;
using Common.Models.Models;
using EasyNetQ;

namespace Common.ServiceBus.EasyNetQRPC
{
    public abstract class BaseEasyNetQRPCustomer
    {
        protected readonly ILogger _logger;
        protected readonly IBus _bus;

        public BaseEasyNetQRPCustomer(ILogger logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        protected async Task<TResponse> RequestHandlerAsync<TResponse, TRequest>(TRequest request, string keyError) 
            where TResponse : ExecutionResult, new()
        {
            return await _bus.Rpc
                .RequestAsync<TRequest, TResponse>(request)
                .ContinueWith(task =>
                {
                    if (task.Status == TaskStatus.Canceled)
                    {
                        _logger.LogError(task.Exception, "Unknown error in BaseEasyNetQRPCustomer");
                        return (TResponse)Activator.CreateInstance(typeof(TResponse), StatusCodeExecutionResult.InternalServer, keyError, "Unknown error!")!;
                    }

                    return task.Result;
                });
        }
    }
}
