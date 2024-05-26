using Microsoft.Extensions.Logging;
using ApplicantService.Core.Application.Interfaces.Services;
using ApplicantService.Core.Domain;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;
using Common.ServiceBus.EasyNetQRPC;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using EasyNetQ;

namespace ApplicantService.Core.Application.Services
{
    public class EasyNetQRequestService : BaseEasyNetQRPCustomer, IRequestService
    {
        public EasyNetQRequestService(ILogger<EasyNetQRequestService> logger, IBus bus) 
            : base(logger, bus) { }

        public async Task<ExecutionResult> CheckPermissionsAsync(Guid applicantId, Guid? managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, CheckPermissionsRequest>(new()
            {
                ApplicantId = applicantId,
                ManagerId = managerId
            }, "CheckPermissionsFail");
        }

        public async Task<ExecutionResult<EducationDocumentTypeCache>> GetEducationDocumentTypeAsync(Guid documentId)
        {
            var response = await RequestHandlerAsync<ExecutionResult<GetEducationDocumentTypeResponse>, GetEducationDocumentTypeRequest>(new()
            {
                DocumentId = documentId,
            }, "GetEducationDocumentTypeFail");

            if (!response.IsSuccess)
            {
                return new(response.StatusCode, errors: response.Errors);
            }

            return new()
            {
                Result = new()
                {
                    Id = response.Result!.EducationDocumentType.Id,
                    Name = response.Result!.EducationDocumentType.Name,
                    Deprecated = response.Result!.Deprecated,
                }
            };
        }
    }
}
