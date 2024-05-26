using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Services;
using Common.Models.DTOs.Applicant;
using Common.Models.Models;
using Common.ServiceBus.EasyNetQRPC;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests;
using EasyNetQ;

namespace ApplicantService.Presentation.Web.RPCHandlers
{
    public class ApplicantRPCHandler : BaseEasyNetQRPCHandler
    {
        public ApplicantRPCHandler(ILogger<ApplicantRPCHandler> logger, IServiceProvider serviceProvider, IBus bus) 
            : base(logger, serviceProvider, bus) { }

        public override void CreateRequestListeners()
        {
            _bus.Rpc.Respond<GetApplicantRequest, ExecutionResult<GetApplicantResponse>>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await GetApplicantAsync(service, request)));

            _bus.Rpc.Respond<GetApplicantInfoRequest, ExecutionResult<GetApplicantInfoResponse>>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await GetApplicantInfoAsync(service, request)));

            _bus.Rpc.Respond<ChangeApplicantInfoRequest, ExecutionResult>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await ChangeApplicantInfoAsync(service, request)));
        }

        private async Task<ExecutionResult<GetApplicantResponse>> GetApplicantAsync(IServiceProvider service, GetApplicantRequest request)
        {
            var _applicantProfileService = service.GetRequiredService<IApplicantProfileService>();

            ExecutionResult<ApplicantAndAddedDocumentTypesDTO> result = await _applicantProfileService.GetApplicantAndAddedDocumentTypesAsync(request.ApplicantId);

            return ResponseHandler(result, applicant => new GetApplicantResponse()
            {
                Id = applicant.Id,
                FullName = applicant.FullName,
                Email = applicant.Email,
                AddedDocumentTypesId = applicant.AddedDocumentTypesId,
            });
        }

        private async Task<ExecutionResult<GetApplicantInfoResponse>> GetApplicantInfoAsync(IServiceProvider service, GetApplicantInfoRequest request)
        {
            var _applicantProfileService = service.GetRequiredService<IApplicantProfileService>();

            ExecutionResult<ApplicantInfo> result = await _applicantProfileService.GetApplicantInfoAsync(request.ApplicantId);

            return ResponseHandler(result, applicant => new GetApplicantInfoResponse() { ApplicantInfo = applicant });
        }

        private async Task<ExecutionResult> ChangeApplicantInfoAsync(IServiceProvider service, ChangeApplicantInfoRequest request)
        {
            var _applicantProfileService = service.GetRequiredService<IApplicantProfileService>();

            return await _applicantProfileService.EditApplicantProfileAsync(new ()
            {
                Birthday = request.Birthday,
                Citizenship = request.Citizenship,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
            }, request.ApplicantId, request.ManagerId);
        }
    }
}
