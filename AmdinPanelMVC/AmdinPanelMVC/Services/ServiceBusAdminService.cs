using AmdinPanelMVC.Services.Base;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs;
using Common.Models.Enums;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;
using EasyNetQ;

namespace AmdinPanelMVC.Services
{
    public class ServiceBusAdminService : BaseRpcService, IAdminService
    {
        public ServiceBusAdminService(IBus bus) : base(bus) { }

        public async Task<ExecutionResult<List<UpdateStatusDTO>>> GetUpdateStatusesAsync()
        {
            ExecutionResult<GetUpdateStatusesResponse> response
               = await RequestHandlerAsync<ExecutionResult<GetUpdateStatusesResponse>, GetUpdateStatusesRequest>(
                    new(), "GetUpdateStatusesFail");

            return ResponseHandler(response, status => status.UpdateStatuses);
        }

        public async Task UpdateAllDictionaryAsync()
        {
            await _bus.PubSub.PublishAsync<UpdateAllDictionaryNotificationRequest>(new());
        }

        public async Task UpdateDictionaryAsync(DictionaryType dictionaryType)
        {
            await _bus.PubSub.PublishAsync<UpdateDictionaryNotificationRequest>(new() { DictionaryType = dictionaryType });
        }
    }
}
