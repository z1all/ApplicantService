using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Mappers;
using AdmissioningService.Core.Domain;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;
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
