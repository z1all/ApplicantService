using AdmissioningService.Core.Domain;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface IRequestService
    {
        Task<ExecutionResult<GetApplicantResponse>> GetApplicantAsync(Guid applicantId);
    }
}
