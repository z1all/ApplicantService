using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    public interface IApplicantCacheRepository : IBaseWithBaseEntityRepository<ApplicantCache>
    {
        Task<ApplicantCache?> GetByIdWithDocumentTypeAndLevelsAsync(Guid applicantId);
    }
}
