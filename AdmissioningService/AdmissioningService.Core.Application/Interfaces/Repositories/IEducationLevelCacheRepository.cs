using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    public interface IEducationLevelCacheRepository : IBaseRepository<EducationLevelCache>
    {
        Task<List<EducationLevelCache>> GetAllAsync();
    }
}
