using AdmissioningService.Core.Application.Helpers;
using Common.Models.Models;
using Common.ServiceBus.EasyNetQRPC;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using EasyNetQ;

namespace AdmissioningService.Presentation.Web.RPCHandlers
{
    public class AdmissionRPCHandler : BaseEasyNetQRPCHandler
    {
        public AdmissionRPCHandler(IServiceProvider serviceProvider, IBus bus) : base(serviceProvider, bus) { }

        public override void CreateRequestListeners()
        {
            _bus.Rpc.Respond<CheckPermissionsRequest, ExecutionResult>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await CheckPermissionsAsync(service, request)));
        }

        public async Task<ExecutionResult> CheckPermissionsAsync(IServiceProvider service, CheckPermissionsRequest request)
        {
            var admissionHelper = service.GetRequiredService<AdmissionHelper>();

            return await admissionHelper.CheckPermissionsAsync(request.ApplicantId, request.ManagerId);
        }
    }
}
