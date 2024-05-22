using ApplicantService.Core.Domain;
using Common.Repositories;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IApplicantRepository : IBaseWithBaseEntityRepository<Applicant> 
    {
        Task<Applicant?> GetByIdWithDocumentsAsync(Guid applicantId);
    }
}
