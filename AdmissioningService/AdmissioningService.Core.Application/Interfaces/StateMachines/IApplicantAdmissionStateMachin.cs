using AdmissioningService.Core.Domain;

namespace AdmissioningService.Core.Application.Interfaces.StateMachines
{
    public interface IApplicantAdmissionStateMachin
    {
        Task AddAsync(Guid applicantId, AdmissionCompany admissionCompany);
        Task<ApplicantAdmission?> GetByAdmissionCompanyIdAndApplicantId(Guid admissionCompanyId, Guid applicantId);
        Task<ApplicantAdmission?> GetByApplicantIdAndAdmissionIdAsync(Guid applicantId, Guid admissionId);
        Task<ApplicantAdmission?> GetCurrentByApplicantIdAsync(Guid applicantId);
        Task<bool> CheckManagerEditPermissionAsync(Guid applicantId, Guid managerId);
        Task<bool> CheckAdmissionStatusIsCloseAsync(Guid applicantId);
    }
}
