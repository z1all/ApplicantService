using AdmissioningService.Core.Application.Interfaces.Services;
using Common.Models.DTOs.Admission;
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

            _bus.Rpc.Respond<ChangedManagerRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await ChangeManagerAsync(service, request)));

            _bus.Rpc.Respond<DeletedManagerRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await DeleteManagerAsync(service, request)));
            
            _bus.Rpc.Respond<GetManagerProfileRequest, ExecutionResult<GetManagerProfileResponse>>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await GetManagerProfileAsync(service, request)));

            _bus.Rpc.Respond<GetManagersRequest, ExecutionResult<GetManagersResponse>>(async (_) =>
                await ExceptionHandlerAsync(async (service) => await GetManagersAsync(service))); 
            
            _bus.Rpc.Respond<AddManagerToAdmissionRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await AddManagerToAdmissionAsync(service, request)));

            _bus.Rpc.Respond<TakeApplicantAdmissionRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await TakeApplicantAdmissionAsync(service, request)));            
            
            _bus.Rpc.Respond<RejectApplicantAdmissionRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await RejectApplicantAdmissionAsync(service, request)));
        }

        private async Task<ExecutionResult> CreateManagerAsync(IServiceProvider service, CreatedManagerRequest request)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            return await managerService.CreateManagerAsync(request.ManagerId, request.Manager);
        }

        private async Task<ExecutionResult> ChangeManagerAsync(IServiceProvider service, ChangedManagerRequest request)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            return await managerService.ChangeManagerAsync(request.ManagerId, request.Manager);
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

        private async Task<ExecutionResult> AddManagerToAdmissionAsync(IServiceProvider service, AddManagerToAdmissionRequest request)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            return await managerService.AddManagerToAdmissionAsync(request.AdmissionId, request.MangerId);
        }

        private async Task<ExecutionResult> TakeApplicantAdmissionAsync(IServiceProvider service, TakeApplicantAdmissionRequest request)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            return await managerService.TakeApplicantAdmissionAsync(request.AdmissionId, request.MangerId);
        }

        private async Task<ExecutionResult> RejectApplicantAdmissionAsync(IServiceProvider service, RejectApplicantAdmissionRequest request)
        {
            var managerService = service.GetRequiredService<IManagerService>();

            return await managerService.RefuseFromApplicantAdmissionAsync(request.AdmissionId, request.MangerId);
        }
    }
}
