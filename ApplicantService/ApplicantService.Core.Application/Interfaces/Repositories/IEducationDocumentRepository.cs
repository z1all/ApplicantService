using ApplicantService.Core.Domain;
using Common.Repositories;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IEducationDocumentRepository : IBaseWithBaseEntityRepository<EducationDocument> 
    { 
        Task<EducationDocument?> GetByDocumentIdAndApplicantIdAsync(Guid documentId, Guid applicantId);
        Task<List<EducationDocument>> GetAllByApplicantIdAsync(Guid applicantId);
        Task<bool> AnyByDocumentTypeIdAndApplicantIdAsync(Guid documentTypeId, Guid applicantId);
    }
}
