using ApplicantService.Core.Domain;
using Common.Repositories;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IEducationDocumentTypeCacheRepository : IBaseRepository<EducationDocumentTypeCache> 
    {
        Task<bool> AnyByIdAsync(Guid id, bool getDeprecated);
    }
}
