using AdmissioningService.Core.Application.Interfaces.Services;
using Common.Models.Models;
using Common.ServiceBus.EasyNetQAutoSubscriber;
using Common.ServiceBus.ServiceBusDTOs.FromUserService;
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
    }
}
