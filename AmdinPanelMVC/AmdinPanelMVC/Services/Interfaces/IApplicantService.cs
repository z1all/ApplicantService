using AmdinPanelMVC.DTOs;
using Common.Models.DTOs.Admission;
using Common.Models.DTOs.Applicant;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IApplicantService
    {
        Task<ExecutionResult<ApplicantInfo>> GetApplicantInfoAsync(Guid applicantId);
        Task<ExecutionResult> ChangeAdditionInfoAsync(ChangeAdditionInfoDTO changeInfo, Guid managerId);
        Task<ExecutionResult> ChangePrioritiesAsync(Guid applicantId, ChangePrioritiesApplicantProgramDTO changePriorities, Guid managerId);
        Task<ExecutionResult> DeleteProgramAsync(Guid applicantId, Guid programId, Guid managerId);
    }
}
