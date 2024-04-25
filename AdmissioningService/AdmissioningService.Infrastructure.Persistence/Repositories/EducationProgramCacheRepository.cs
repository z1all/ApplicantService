using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class EducationProgramCacheRepository : BaseRepository<EducationProgramCache, AppDbContext>, IEducationProgramCacheRepository
    {
        public EducationProgramCacheRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
