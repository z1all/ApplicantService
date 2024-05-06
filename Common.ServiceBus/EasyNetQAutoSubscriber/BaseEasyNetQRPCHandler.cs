using Microsoft.Extensions.DependencyInjection;
using EasyNetQ;
using Common.Models.Models;

namespace Common.ServiceBus.EasyNetQAutoSubscriber
{
    public abstract class BaseEasyNetQRPCHandler
    {
        private readonly IServiceProvider _serviceProvider;
        protected readonly IBus _bus;

        public BaseEasyNetQRPCHandler(IServiceProvider serviceProvider, IBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        public abstract void CreateRequestListeners();

        protected async Task<ExecutionResult> ExceptionHandlerAsync(Func<IServiceProvider, Task<ExecutionResult>> operationAsync) =>
             await ExceptionHandlerAsync(operationAsync, () => new(keyError: "UnknownError", error: "Unknown error"));
        

        protected async Task<ExecutionResult<TResponse>> ExceptionHandlerAsync<TResponse>(Func<IServiceProvider, Task<ExecutionResult<TResponse>>> operationAsync) =>
             await ExceptionHandlerAsync(operationAsync, () => new(keyError: "UnknownError", error: "Unknown error"));

        private async Task<TResponse> ExceptionHandlerAsync<TResponse>(Func<IServiceProvider, Task<TResponse>> operationAsync, Func<TResponse> errorResponse)
        {
            try
            {
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    return await operationAsync(scope.ServiceProvider);
                }
            }
            catch
            {
                return errorResponse();
            }
        }
    }
}