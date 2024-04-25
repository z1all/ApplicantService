using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface IRequestService
    {
        Task<ExecutionResult<GetApplicantResponse>> GetApplicantAsync(Guid applicantId);
        Task<ExecutionResult<GetFacultyResponse>> GetFacultyAsync(Guid facultyId);
    }
}
