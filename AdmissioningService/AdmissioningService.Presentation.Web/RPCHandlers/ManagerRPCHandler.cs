using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Presentation.Web.RPCHandlers.Mappers;
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
            _bus.Rpc.Respond<CreateManagerRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await CreateManagerAsync(service, request)));

            _bus.Rpc.Respond<DeleteManagerRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await DeleteManagerAsync(service, request)));
            
            _bus.Rpc.Respond<GetManagerProfileRequest, ExecutionResult<GetManagerProfileResponse>>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await GetManagerProfileAsync(service, request)));
        }

        private async Task<ExecutionResult> CreateManagerAsync(IServiceProvider service, CreateManagerRequest request)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            return await managerService.CreateManagerAsync(new()
            {
                Id = request.Id,
                FullName = request.FullName,
                Email = request.Email,
                FacultyId = request.FacultyId,
            });
        }

        private async Task<ExecutionResult> DeleteManagerAsync(IServiceProvider service, DeleteManagerRequest request)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            return await managerService.DeleteManagerAsync(request.ManagerId);
        }

        private async Task<ExecutionResult<GetManagerProfileResponse>> GetManagerProfileAsync(IServiceProvider service, GetManagerProfileRequest request)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            ExecutionResult<ManagerDTO> response = await managerService.GetManagerAsync(request.ManagerId);

            return ResponseHandler(response, manager => manager.ToGetManagerProfileResponse());
        }
    }
}
