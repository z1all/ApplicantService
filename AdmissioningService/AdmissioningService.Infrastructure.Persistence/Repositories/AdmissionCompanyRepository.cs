using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class AdmissionCompanyRepository : BaseRepository<AdmissionCompany, AppDbContext>, IAdmissionCompanyRepository
    {
        public AdmissionCompanyRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
