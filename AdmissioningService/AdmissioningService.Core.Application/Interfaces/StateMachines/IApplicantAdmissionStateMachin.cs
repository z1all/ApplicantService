using AdmissioningService.Core.Domain;

namespace AdmissioningService.Core.Application.Interfaces.StateMachines
{
    public interface IApplicantAdmissionStateMachin
    {
        Task AddAsync(Guid applicantId, AdmissionCompany admissionCompany);
        Task UpdateAsync(ApplicantAdmission entity, bool isUpdated = true);
    }
}
