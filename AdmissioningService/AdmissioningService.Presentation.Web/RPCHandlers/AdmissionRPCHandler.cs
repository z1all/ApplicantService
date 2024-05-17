using AdmissioningService.Core.Application.Helpers;
using AdmissioningService.Core.Application.Interfaces.Services;
using Common.Models.DTOs.Admission;
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

            _bus.Rpc.Respond<GetAdmissionsRequest, ExecutionResult>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await GetAdmissionsAsync(service, request)));

            _bus.Rpc.Respond<GetApplicantAdmissionRequest, ExecutionResult<GetApplicantAdmissionResponse>>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await GetApplicantAdmissionAsync(service, request)));

            _bus.Rpc.Respond<ChangeAdmissionStatusRequest, ExecutionResult>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await ChangeAdmissionStatusAsync(service, request)));
        }

        public async Task<ExecutionResult> CheckPermissionsAsync(IServiceProvider service, CheckPermissionsRequest request)
        {
            var admissionHelper = service.GetRequiredService<AdmissionHelper>();

            return await admissionHelper.CheckPermissionsAsync(request.ApplicantId, request.ManagerId);
        }

        public async Task<ExecutionResult<GetAdmissionsResponse>> GetAdmissionsAsync(IServiceProvider service, GetAdmissionsRequest request)
        {
            var _admissionService = service.GetRequiredService<IAdmissionService>();

            ExecutionResult<ApplicantAdmissionPagedDTO> result = await _admissionService.GetApplicantAdmissionsAsync(request.ApplicantAdmissionFilter, request.ManagerId);

            return ResponseHandler(result, admissions => new GetAdmissionsResponse() { ApplicantAdmissionPaged = admissions });
        }

        public async Task<ExecutionResult<GetApplicantAdmissionResponse>> GetApplicantAdmissionAsync(IServiceProvider service, GetApplicantAdmissionRequest request)
        {
            var _admissionService = service.GetRequiredService<IAdmissionService>();

            ExecutionResult<ApplicantAdmissionDTO> result = await _admissionService.GetApplicantAdmissionAsync(request.ApplicantId, request.AdmissionId);

            return ResponseHandler(result, admissions => new GetApplicantAdmissionResponse() { ApplicantAdmission = admissions });
        }

        public async Task<ExecutionResult> ChangeAdmissionStatusAsync(IServiceProvider service, ChangeAdmissionStatusRequest request)
        {
            var _managerService = service.GetRequiredService<IManagerService>();

            return await _managerService.ChangeApplicantAdmissionStatusAsync(request.AdmissionId, request.NewStatus, request.ManagerId);
        }
    }
}
