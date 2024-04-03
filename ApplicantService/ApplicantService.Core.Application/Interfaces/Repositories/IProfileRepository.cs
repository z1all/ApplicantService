using ApplicantService.Core.Domain;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IProfileRepository
    {
        Task<Applicant?> GetByIdAsync(Guid id);
        Task<bool> AnyByIdAsync(Guid id);
        Task CreateAsync(UserCache user, Applicant applicant);
        Task UpdateAsync(Applicant applicant);
        Task UpdateAsync(UserCache user);
    }
}
