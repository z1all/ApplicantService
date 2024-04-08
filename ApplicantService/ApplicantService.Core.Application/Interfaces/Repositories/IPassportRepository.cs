using ApplicantService.Core.Domain;
using Common.Repositories;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IPassportRepository : IBaseRepository<Passport> 
    {
        Task<Passport?> GetByApplicantIdAsync(Guid applicantId);
        Task<bool> AnyByApplicantIdAsync(Guid applicantId);
    }
}
