using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Domain;
using Common.Models.Models;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface IApplicantProfileService
    {
        Task<ExecutionResult<ApplicantProfile>> GetApplicantProfileAsync(Guid applicantId);
        Task<ExecutionResult> EditApplicantProfileAsync(EditApplicantProfile applicantProfile, Guid applicantId);
        Task<ExecutionResult<ApplicantAndAddedDocumentTypesDTO>> GetApplicantAndAddedDocumentTypesAsync(Guid applicantId);
        Task CreateApplicantAsync(UserDTO user);
        Task UpdateApplicantAsync(UserDTO newUser);
    }
}
