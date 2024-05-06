using AdmissioningService.Core.Application.DTOs;
using Common.Models.Models;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface IAdmissionService
    {
        Task<ExecutionResult<List<AdmissionCompanyDTO>>> GetAdmissionCompaniesAsync(Guid applicantId);
        Task<ExecutionResult> CreateAdmissionAsync(Guid applicantId);
        Task<ExecutionResult<ApplicantAdmissionDTO>> GetApplicantAdmissionAsync(Guid applicantId, Guid admissionId);
        Task<ExecutionResult> AddProgramToCurrentAdmissionAsync(Guid applicantId, Guid programId, Guid? managerId = null);
        Task<ExecutionResult> ChangeAdmissionProgramPriorityAsync(Guid applicantId, ChangePrioritiesApplicantProgramDTO changePriorities, Guid? managerId = null);
        Task<ExecutionResult> DeleteAdmissionProgramAsync(Guid applicantId, Guid programId, Guid? managerId = null);

        Task<ExecutionResult<ApplicantAdmissionPagedDTO>> GetApplicantAdmissionsAsync(ApplicantAdmissionFilterDTO admissionFilter, Guid managerId);
    }
}
