using AdmissioningService.Core.Application.Interfaces.Services;
using Common.Models.Models;
using Common.ServiceBus.EasyNetQRPC;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;
using EasyNetQ;

namespace AdmissioningService.Core.Application.Services
{
    public class EasyNetQRequestService : BaseEasyNetQRPCustomer, IRequestService
    {
        public EasyNetQRequestService(IBus bus) : base(bus) { }

        public async Task<ExecutionResult<GetApplicantResponse>> GetApplicantAsync(Guid applicantId)
        {
            return await RequestHandlerAsync<ExecutionResult<GetApplicantResponse>, GetApplicantRequest>(
                new() { ApplicantId = applicantId }, "GetApplicantFail");
        }

        public async Task<ExecutionResult<GetEducationDocumentTypeResponse>> GetEducationDocumentTypeAsync(Guid documentTypId)
        {
            return await RequestHandlerAsync<ExecutionResult<GetEducationDocumentTypeResponse>, GetEducationDocumentTypeRequest>(
                new() { DocumentId = documentTypId }, "GetEducationDocumentTypeFail");
        }

        public async Task<ExecutionResult<GetEducationProgramResponse>> GetEducationProgramAsync(Guid programId)
        {
            return await RequestHandlerAsync<ExecutionResult<GetEducationProgramResponse>, GetEducationProgramRequest>(
                new() { ProgramId = programId }, "GetEducationProgramFail");
        }

        public async Task<ExecutionResult<GetFacultyResponse>> GetFacultyAsync(Guid facultyId)
        {
            return await RequestHandlerAsync<ExecutionResult<GetFacultyResponse>, GetFacultyRequest>(
                new() { FacultyId = facultyId}, "GetFacultyFail");
        }
    }
}
