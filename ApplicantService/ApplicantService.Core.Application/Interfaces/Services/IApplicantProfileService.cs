using ApplicantService.Core.Application.DTOs;
using Common.Models.Models;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface IApplicantProfileService
    {
        Task<ExecutionResult<ApplicantProfile>> GetApplicantProfileAsync(Guid applicantId);
        Task<ExecutionResult> EditApplicantProfileAsync(EditApplicantProfile applicantProfile, Guid applicantId);
    }
}
