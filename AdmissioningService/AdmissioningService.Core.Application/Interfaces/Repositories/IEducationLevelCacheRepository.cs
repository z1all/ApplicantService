using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    public interface IEducationLevelCacheRepository : IBaseWithBaseEntityRepository<EducationLevelCache>
    {
        Task<List<EducationLevelCache>> GetAllAsync();
    }
}
