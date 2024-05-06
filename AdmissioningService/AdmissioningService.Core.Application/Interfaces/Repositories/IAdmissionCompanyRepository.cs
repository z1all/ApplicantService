using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    public interface IAdmissionCompanyRepository : IBaseWithBaseEntityRepository<AdmissionCompany>
    {
        Task<List<AdmissionCompany>> GetAllWithApplicantAdmissionAsync(Guid applicantId);
        Task<AdmissionCompany?> GetCurrentAsync();
    }
}
