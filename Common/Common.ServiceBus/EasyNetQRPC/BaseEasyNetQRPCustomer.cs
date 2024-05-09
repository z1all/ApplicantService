using Common.Models.Models;
using EasyNetQ;

namespace Common.ServiceBus.EasyNetQRPC
{
    public abstract class BaseEasyNetQRPCustomer
    {
        protected readonly IBus _bus;

        public BaseEasyNetQRPCustomer(IBus bus)
        {
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
                        return (TResponse)Activator.CreateInstance(typeof(TResponse), keyError, "Unknown error!")!;
                    }

                    return task.Result;
                });
        }
    }
}
