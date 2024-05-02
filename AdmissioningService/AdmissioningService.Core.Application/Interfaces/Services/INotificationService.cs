using AdmissioningService.Core.Application.DTOs;
using Common.Models.Models;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<ExecutionResult> AddedManagerToApplicantAdmission(UserDTO manager, UserDTO applicant);
    }
}
