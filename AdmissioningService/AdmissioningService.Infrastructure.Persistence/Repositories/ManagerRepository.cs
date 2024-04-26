using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class ManagerRepository : BaseWithBaseEntityRepository<Manager, AppDbContext>, IManagerRepository
    {
        public ManagerRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
