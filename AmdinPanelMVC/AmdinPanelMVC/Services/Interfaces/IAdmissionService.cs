using Common.Models.DTOs.Admission;
using Common.Models.Enums;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IAdmissionService
    {
        Task<ExecutionResult<List<AdmissionCompanyDTO>>> GetAdmissionsCompaniesAsync();
        Task<ExecutionResult> CreateAdmissionCompanyAsync(int year);
        Task<ExecutionResult<ApplicantAdmissionPagedDTO>> GetAdmissionsAsync(ApplicantAdmissionFilterDTO applicantAdmission, Guid managerId);
        Task<ExecutionResult<ApplicantAdmissionDTO>> GetApplicantAdmissionAsync(Guid applicantId, Guid admissionId);

        Task<ExecutionResult> AddManagerToAdmissionAsync(Guid admissionId, Guid? managerId);
        Task<ExecutionResult> TakeApplicantAdmissionAsync(Guid admissionId, Guid managerId);
        Task<ExecutionResult> RejectApplicantAdmissionAsync(Guid admissionId, Guid managerId);

        Task<ExecutionResult> ChangeAdmissionStatusAsync(Guid admissionId, Guid managerId, ManagerChangeAdmissionStatus changeStatus);
    }
}
