using Microsoft.EntityFrameworkCore;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class FacultyCacheRepository : BaseRepository<FacultyCache, AppDbContext>, IFacultyCacheRepository
    {
        public FacultyCacheRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
