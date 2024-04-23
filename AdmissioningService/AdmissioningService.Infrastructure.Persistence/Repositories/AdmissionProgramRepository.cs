using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class AdmissionProgramRepository : BaseRepository<AppDbContext>, IAdmissionProgramRepository
    {
        public AdmissionProgramRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
