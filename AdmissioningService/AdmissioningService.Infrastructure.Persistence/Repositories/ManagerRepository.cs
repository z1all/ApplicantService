using Microsoft.EntityFrameworkCore;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class ManagerRepository : BaseWithBaseEntityRepository<Manager, AppDbContext>, IManagerRepository
    {
        public ManagerRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Manager?> GetByIdWithUserAsync(Guid managerId)
        {
            return await _dbContext.Managers
                .Include(manager => manager.User)
                .FirstOrDefaultAsync(manager => manager.Id == managerId);
        }

        public async Task<List<Manager>> GetAllWithFacultyAndUserAsync()
        {
            return await _dbContext.Managers
                .Include(manager => manager.Faculty)
                .Include(manager => manager.User)
                .ToListAsync();
        }
    }
}
