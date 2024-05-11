using AmdinPanelMVC.Services.Base;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs;
using Common.Models.Enums;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;
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

        public async Task<ExecutionResult<List<ManagerProfileDTO>>> GetManagersAsync()
        {
            ExecutionResult<GetManagersResponse> response
               = await RequestHandlerAsync<ExecutionResult<GetManagersResponse>, GetManagersRequest>(
                    new(), "GetUpdateStatusesFail");

            return ResponseHandler(response, managers => managers.Managers);
        }

        public Task<ExecutionResult> ChangeManagerAsync(Guid managerId, ManagerDTO manager)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecutionResult> AddManagerAsync(ManagerDTO manager, string password)
        {
            return await _bus.Rpc.RequestAsync<CreateNewManagerRequest, ExecutionResult>(
                new() { Manager = manager, Password = password });
        }

        public async Task<ExecutionResult> DeleteManagerAsync(Guid managerId)
        {
            return await _bus.Rpc.RequestAsync<DeleteManagerRequest, ExecutionResult>(
                new() { ManagerId = managerId });
        }
    }
}
