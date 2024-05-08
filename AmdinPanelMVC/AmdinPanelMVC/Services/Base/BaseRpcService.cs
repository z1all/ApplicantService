using Common.Models.Models;
using Common.ServiceBus.EasyNetQRPC;
using EasyNetQ;

namespace AmdinPanelMVC.Services.Base
{
    public abstract class BaseRpcService : BaseEasyNetQRPCustomer
    {
        protected BaseRpcService(IBus bus) : base(bus) { }

        protected ExecutionResult<TToResult> ResponseHandler<TToResult, TFromResult>(
            ExecutionResult<TFromResult> response,
            Func<TFromResult, TToResult> mapper)
        {
            if (!response.IsSuccess) return new() { Errors = response.Errors };

            return new() { Result = mapper(response.Result!) };
        }
    }
}
