using ApplicantService.Core.Application.DTOs;
using Common.Models.DTOs.Applicant;
using Common.Models.Models;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface IApplicantProfileService
    {
        Task<ExecutionResult<ApplicantProfile>> GetApplicantProfileAsync(Guid applicantId);
        Task<ExecutionResult> EditApplicantProfileAsync(EditApplicantProfile applicantProfile, Guid applicantId, Guid? managerId = null);
        Task<ExecutionResult<ApplicantAndAddedDocumentTypesDTO>> GetApplicantAndAddedDocumentTypesAsync(Guid applicantId);
        Task<ExecutionResult<ApplicantInfo>> GetApplicantInfoAsync(Guid applicantId);
        Task CreateApplicantAsync(UserDTO user);
        Task UpdateApplicantAsync(UserDTO newUser);
    }
}
