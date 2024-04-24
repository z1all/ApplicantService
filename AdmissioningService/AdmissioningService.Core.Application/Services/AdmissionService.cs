using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Application.Interfaces.Services;
using Common.Models.Models;

namespace AdmissioningService.Core.Application.Services
{
    public class AdmissionService : IAdmissionService
    {
        public Task<ExecutionResult> AddProgramToAdmissionAsync(Guid applicantId, Guid admissionId, Guid programId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> ChangeAdmissionProgramPriorityAsync(Guid applicantId, Guid admissionId, ChangePrioritiesApplicantProgramDTO changePriorities)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> CreateReceptionCompanyAsync(Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> DeleteAdmissionProgramAsync(Guid applicantId, Guid admissionId, Guid programId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult<ApplicantAdmissionDTO>> GetApplicantAdmissionAsync(Guid applicantId, Guid admissionId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult<List<AdmissionCompanyDTO>>> GetReceptionCompaniesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
