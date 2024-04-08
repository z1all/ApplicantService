using ApplicantService.Core.Domain;
using Common.Repositories;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IEducationDocumentRepository : IBaseRepository<EducationDocument> 
    { 
        Task<EducationDocument?> GetByDocumentIdAndApplicantIdAsync(Guid documentId, Guid applicantId);
        Task<bool> AnyByDocumentIdAndApplicantIdAsync(Guid documentId, Guid applicantId);
    }
}
