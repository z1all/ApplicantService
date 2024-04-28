using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Services;
using Common.Models.Models;
using Common.ServiceBus.EasyNetQAutoSubscriber;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;
using EasyNetQ;

namespace ApplicantService.Presentation.Web.RPCHandlers
{
    public class ApplicantRPCHandler : BaseEasyNetQRPCHandler
    {
        public ApplicantRPCHandler(IServiceProvider serviceProvider, IBus bus) : base(serviceProvider, bus) { }

        public override void CreateRequestListeners()
        {
            _bus.Rpc.Respond<GetApplicantRequest, ExecutionResult<GetApplicantResponse>>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await GetApplicantAsync(service, request)));
        }

        private async Task<ExecutionResult<GetApplicantResponse>> GetApplicantAsync(IServiceProvider service, GetApplicantRequest request)
        {
            var _applicantProfileService = service.GetRequiredService<IApplicantProfileService>();

            ExecutionResult<ApplicantAndAddedDocumentTypesDTO> result = await _applicantProfileService.GetApplicantAndAddedDocumentTypesAsync(request.ApplicantId);
            if (!result.IsSuccess) return new() { Errors = result.Errors };
            ApplicantAndAddedDocumentTypesDTO applicant = result.Result!;

            return new()
            {
                Result = new()
                {
                    Id = applicant.Id,
                    FullName = applicant.FullName,
                    Email = applicant.Email,
                    AddedDocumentTypesId = applicant.AddedDocumentTypesId,
                }
            };
        }
    }
}
