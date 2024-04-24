using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    [Obsolete("Don't use this repository, use IApplicantAdmissionStateMachin")]
    public interface IApplicantAdmissionRepository : IBaseRepository<ApplicantAdmission>
    {
        Task<ApplicantAdmission?> GetByAdmissionCompanyId(Guid admissionCompanyId);
    }
}
