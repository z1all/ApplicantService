using AdmissioningService.Core.Application.Helpers;
using AdmissioningService.Core.Application.Interfaces.Services;
using Common.Models.DTOs;
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

            _bus.Rpc.Respond<GetAdmissionsAsyncRequest, ExecutionResult>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await GetAdmissionsAsync(service, request)));
        }

        public async Task<ExecutionResult> CheckPermissionsAsync(IServiceProvider service, CheckPermissionsRequest request)
        {
            var admissionHelper = service.GetRequiredService<AdmissionHelper>();

            return await admissionHelper.CheckPermissionsAsync(request.ApplicantId, request.ManagerId);
        }

        public async Task<ExecutionResult<GetAdmissionsAsyncResponse>> GetAdmissionsAsync(IServiceProvider service, GetAdmissionsAsyncRequest request)
        {
            var _admissionService = service.GetRequiredService<IAdmissionService>();

            ExecutionResult<ApplicantAdmissionPagedDTO> result = await _admissionService.GetApplicantAdmissionsAsync(request.ApplicantAdmissionFilter, request.ManagerId);

            return ResponseHandler(result, admissions => new GetAdmissionsAsyncResponse() { ApplicantAdmissionPaged = admissions });
        }
    }
}
