using ApplicantService.Core.Domain;
using Common.Repositories;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IDocumentFileInfoRepository : IBaseWithBaseEntityRepository<DocumentFileInfo>
    {
        Task<List<DocumentFileInfo>> GetAllByApplicantIdAndDocumentId(Guid applicantId, Guid documentId);
    }
}
