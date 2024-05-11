using AdmissioningService.Core.Application.Interfaces.Services;
using Common.Models.DTOs;
using Common.Models.Models;
using Common.ServiceBus.EasyNetQRPC;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;
using EasyNetQ;

namespace AdmissioningService.Presentation.Web.RPCHandlers
{
    public class ManagerRPCHandler : BaseEasyNetQRPCHandler
    {
        public ManagerRPCHandler(IServiceProvider serviceProvider, IBus bus) : base(serviceProvider, bus) { }

        public override void CreateRequestListeners()
        {
            _bus.Rpc.Respond<CreatedManagerRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await CreateManagerAsync(service, request)));

            _bus.Rpc.Respond<DeletedManagerRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await DeleteManagerAsync(service, request)));
            
            _bus.Rpc.Respond<GetManagerProfileRequest, ExecutionResult<GetManagerProfileResponse>>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await GetManagerProfileAsync(service, request)));

            _bus.Rpc.Respond<GetManagersRequest, ExecutionResult<GetManagersResponse>>(async (_) =>
                await ExceptionHandlerAsync(async (service) => await GetManagersAsync(service)));
        }

        private async Task<ExecutionResult> CreateManagerAsync(IServiceProvider service, CreatedManagerRequest request)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            return await managerService.CreateManagerAsync(request.Id, new()
            {
                FullName = request.FullName,
                Email = request.Email,
                FacultyId = request.FacultyId,
            });
        }

        private async Task<ExecutionResult> DeleteManagerAsync(IServiceProvider service, DeletedManagerRequest request)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            return await managerService.DeleteManagerAsync(request.ManagerId);
        }

        private async Task<ExecutionResult<GetManagerProfileResponse>> GetManagerProfileAsync(IServiceProvider service, GetManagerProfileRequest request)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            ExecutionResult<ManagerProfileDTO> response = await managerService.GetManagerAsync(request.ManagerId);

            return ResponseHandler(response, manager => new GetManagerProfileResponse() { Manager = manager });
        }

        private async Task<ExecutionResult<GetManagersResponse>> GetManagersAsync(IServiceProvider service)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            ExecutionResult<List<ManagerProfileDTO>> response = await managerService.GetManagersAsync();

            return ResponseHandler(response, managers => new GetManagersResponse() { Managers = managers });
        }
    }
}
