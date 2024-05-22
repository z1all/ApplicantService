using ApplicantService.Core.Domain;
using Common.Repositories;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IEducationDocumentTypeCacheRepository : IBaseWithBaseEntityRepository<EducationDocumentTypeCache> 
    {
        Task<bool> AnyByIdAsync(Guid id, bool getDeprecated);
    }
}
