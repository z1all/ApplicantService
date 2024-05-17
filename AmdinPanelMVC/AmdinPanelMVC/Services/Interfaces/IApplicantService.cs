using AmdinPanelMVC.DTOs;
using Common.Models.DTOs.Applicant;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IApplicantService
    {
        Task<ExecutionResult<ApplicantInfo>> GetApplicantInfoAsync(Guid applicantId);
        Task<ExecutionResult> ChangeAdditionInfoAsync(ChangeAdditionInfoDTO changeInfo, Guid managerId);
    }
}
