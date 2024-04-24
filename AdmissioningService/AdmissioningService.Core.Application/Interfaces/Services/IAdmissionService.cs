using AdmissioningService.Core.Application.DTOs;
using Common.Models.Models;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface IAdmissionService
    {
        Task<ExecutionResult<List<AdmissionCompanyDTO>>> GetAdmissionCompaniesAsync(Guid applicantId);
        Task<ExecutionResult> CreateAdmissionAsync(Guid applicantId);
        Task<ExecutionResult<ApplicantAdmissionDTO>> GetApplicantAdmissionAsync(Guid applicantId, Guid admissionId);
        Task<ExecutionResult> AddProgramToAdmissionAsync(Guid applicantId, Guid admissionId, Guid programId);
        Task<ExecutionResult> ChangeAdmissionProgramPriorityAsync(Guid applicantId, Guid admissionId, ChangePrioritiesApplicantProgramDTO changePriorities);
        Task<ExecutionResult> DeleteAdmissionProgramAsync(Guid applicantId, Guid admissionId, Guid programId);
    }
}
