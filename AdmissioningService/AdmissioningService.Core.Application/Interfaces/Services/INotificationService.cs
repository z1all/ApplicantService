using AdmissioningService.Core.Application.DTOs;
using Common.Models.Enums;
using Common.Models.Models;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<ExecutionResult> AddedManagerToApplicantAdmissionAsync(UserDTO manager, UserDTO applicant);
        Task<ExecutionResult> UpdatedAdmissionStatusAsync(AdmissionStatus newStatus, UserDTO applicant);
    }
}
