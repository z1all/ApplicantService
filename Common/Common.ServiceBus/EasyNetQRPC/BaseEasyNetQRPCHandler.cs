using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Common.Models.Models;
using EasyNetQ;

namespace Common.ServiceBus.EasyNetQRPC
{
    public abstract class BaseEasyNetQRPCHandler
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        protected readonly IBus _bus;

        public BaseEasyNetQRPCHandler(ILogger logger, IServiceProvider serviceProvider, IBus bus)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        public abstract void CreateRequestListeners();

        protected ExecutionResult<TToResult> ResponseHandler<TToResult, TFromResult>(
           ExecutionResult<TFromResult> response,
           Func<TFromResult, TToResult> mapper)
        {
            if (!response.IsSuccess) return new(response.StatusCode, errors: response.Errors);

            return new() { Result = mapper(response.Result!) };
        }

        protected async Task<ExecutionResult> ExceptionHandlerAsync(Func<IServiceProvider, Task<ExecutionResult>> operationAsync) =>
             await ExceptionHandlerAsync(operationAsync, () => new(StatusCodeExecutionResult.InternalServer, keyError: "UnknownError", error: "Unknown error"));

        protected async Task<ExecutionResult<TResponse>> ExceptionHandlerAsync<TResponse>(Func<IServiceProvider, Task<ExecutionResult<TResponse>>> operationAsync) =>
             await ExceptionHandlerAsync(operationAsync, () => new(StatusCodeExecutionResult.InternalServer, keyError: "UnknownError", error: "Unknown error"));

        private async Task<TResponse> ExceptionHandlerAsync<TResponse>(Func<IServiceProvider, Task<TResponse>> operationAsync, Func<TResponse> errorResponse)
        {
            try
            {
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    return await operationAsync(scope.ServiceProvider);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknow error in BaseEasyNetQRPCHandler");
                return errorResponse();
            }
        }
    }
}