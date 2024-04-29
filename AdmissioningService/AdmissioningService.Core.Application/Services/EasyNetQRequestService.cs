using AdmissioningService.Core.Application.Interfaces.Services;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;
using EasyNetQ;

namespace AdmissioningService.Core.Application.Services
{
    public class EasyNetQRequestService : IRequestService
    {
        private readonly IBus _bus;

        public EasyNetQRequestService(IBus bus)
        {
            _bus = bus;
        }

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

        private async Task<TResponse> RequestHandlerAsync<TResponse, TRequest>(TRequest request, string keyError) where TResponse : ExecutionResult, new()
        {
            return await _bus.Rpc
                .RequestAsync<TRequest, TResponse>(request)
                .ContinueWith(task =>
                {
                    if (task.Status == TaskStatus.Canceled)
                    {
                        return (TResponse)Activator.CreateInstance(typeof(TResponse), keyError, "Unknown error!")!;
                    }

                    return task.Result;
                });
        }
    }
}
