using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    public interface IEducationDocumentTypeCacheRepository : IBaseWithBaseEntityRepository<EducationDocumentTypeCache>
    {
        Task<List<EducationDocumentTypeCache>> GetAllByNextEducationLevelWithNextLevelId(Guid nextEducationLevelId);
    }
}
