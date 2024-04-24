using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
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
            var _applicantRepository = service.GetRequiredService<IApplicantRepository>();

            Applicant? applicant = await _applicantRepository.GetByIdAsync(request.ApplicantId);
            if (applicant is null)
            {
                return new(keyError: "ApplicantNotFound", error: $"Applicant with id {request.ApplicantId} not found!");
            }

            var _educationDocumentRepository = service.GetRequiredService<IEducationDocumentRepository>();

            List<EducationDocument> educationDocuments = await _educationDocumentRepository.GetAllByApplicantIdAsync(request.ApplicantId);

            return new()
            {
                Result = new()
                {
                    Id = applicant.Id,
                    FullName = applicant.FullName,
                    Email = applicant.Email,
                    AddedDocumentTypesId = educationDocuments.Select(document => document.EducationDocumentType!.Id).ToList()
                },
            };
        }
    }
}
