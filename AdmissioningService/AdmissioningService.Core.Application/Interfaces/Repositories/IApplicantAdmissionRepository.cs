using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    [Obsolete("Don't use this repository, use IApplicantAdmissionStateMachin")]
    public interface IApplicantAdmissionRepository : IBaseWithBaseEntityRepository<ApplicantAdmission>
    {
        Task<ApplicantAdmission?> GetByAdmissionCompanyIdAndApplicantId(Guid admissionCompanyId, Guid applicantId);
        Task<ApplicantAdmission?> GetCurrentByApplicantId(Guid applicantId);
        Task<ApplicantAdmission?> GetByApplicantIdAndAdmissionIdAsync(Guid applicantId, Guid admissionId);
        Task<bool> AnyByApplicantIdAndAdmissionIdAsync(Guid applicantId, Guid admissionId);
    }
}
