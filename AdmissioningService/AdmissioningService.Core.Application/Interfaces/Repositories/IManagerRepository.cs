using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    public interface IManagerRepository : IBaseWithBaseEntityRepository<Manager>
    {
        Task<Manager?> GetByIdWithUserAsync(Guid managerId);
        Task<Manager?> GetByIdWithFacultyAndUserAsync(Guid managerId);
        Task<List<Manager>> GetAllWithFacultyAndUserAsync();
    }
}
