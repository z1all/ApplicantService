using Common.Models.DTOs;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IAdmissionService
    {
        Task<ExecutionResult<ApplicantAdmissionPagedDTO>> GetApplicantAdmissionAsync(ApplicantAdmissionFilterDTO applicantAdmission, Guid managerId);
    }
}
