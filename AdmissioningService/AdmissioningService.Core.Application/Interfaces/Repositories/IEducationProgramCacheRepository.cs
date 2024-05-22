using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    public interface IEducationProgramCacheRepository : IBaseWithBaseEntityRepository<EducationProgramCache>
    {
        Task<EducationProgramCache?> GetByIdWithoutLevelAsync(Guid programId);
        Task<EducationProgramCache?> GetByIdWithLevelAsync(Guid programId);
    }
}
