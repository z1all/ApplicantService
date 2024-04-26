using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    [Obsolete("Don't use this repository, use IApplicantAdmissionStateMachin")]
    public interface IApplicantAdmissionRepository : IBaseWithBaseEntityRepository<ApplicantAdmission>
    {
        Task<ApplicantAdmission?> GetByAdmissionCompanyId(Guid admissionCompanyId);
        Task<ApplicantAdmission?> GetByApplicantIdAndAdmissionIdAsync(Guid applicantId, Guid admissionId);
        Task<bool> AnyByApplicantIdAndAdmissionIdAsync(Guid applicantId, Guid admissionId);
    }
}
