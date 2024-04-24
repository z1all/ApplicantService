using DictionaryService.Core.Application.Interfaces.Services;
using Common.ServiceBus.EasyNetQAutoSubscriber;
using Common.Models.Models;
using Common.Models.DTOs;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;
using EasyNetQ;

namespace DictionaryService.Presentation.Web.RPCHandlers
{
    public class UpdateDictionaryRPCHandler : BaseEasyNetQRPCHandler
    {
        public UpdateDictionaryRPCHandler(IServiceProvider serviceProvider, IBus bus) : base(serviceProvider, bus) { }

        public override void CreateRequestListeners()
        {
            _bus.Rpc.RespondAsync<GetUpdateStatusesRequest, ExecutionResult<GetUpdateStatusesResponse>>(async (_) =>
                await ExceptionHandlerAsync(GetUpdateStatusesAsync));
        }

        private async Task<ExecutionResult<GetUpdateStatusesResponse>> GetUpdateStatusesAsync(IServiceProvider service)
        {
            var updateDictionaryService = service.GetRequiredService<IUpdateDictionaryService>();

            ExecutionResult<List<UpdateStatusDTO>> updateStatuses = await updateDictionaryService.GetUpdateStatusesAsync();
            if (!updateStatuses.IsSuccess)
            {
                return new() { Errors = updateStatuses.Errors };
            }

            return new()
            {
                Result = new()
                {
                    UpdateStatuses = updateStatuses.Result!
                }
            };
        }
    }
}
