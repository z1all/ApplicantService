using AdmissioningService.Core.Domain;

namespace AdmissioningService.Core.Application.Interfaces.StateMachines
{
    public interface IApplicantAdmissionStateMachin
    {
        Task AddAsync(Guid applicantId, AdmissionCompany admissionCompany);
        Task<ApplicantAdmission?> GetByAdmissionCompanyId(Guid admissionCompanyId);
        Task<ApplicantAdmission?> GetByApplicantIdAndAdmissionIdAsync(Guid applicantId, Guid admissionId);
        Task<bool> AnyByApplicantIdAndAdmissionIdAsync(Guid applicantId, Guid admissionId);
    }
}
