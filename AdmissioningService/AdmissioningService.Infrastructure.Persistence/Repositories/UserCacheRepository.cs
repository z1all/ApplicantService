using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class UserCacheRepository : BaseRepository<UserCache, AppDbContext>, IUserCacheRepository
    {
        public UserCacheRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
