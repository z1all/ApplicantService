using ApplicantService.Core.Domain;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IProfileRepository
    {
        Task<Applicant?> GetByIdAsync(Guid id);
        Task UpdateAsync(Applicant applicant);
    }
}
