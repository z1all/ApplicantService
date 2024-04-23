using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class ApplicantAdmissionRepository : BaseRepository<ApplicantAdmission, AppDbContext>, IApplicantAdmissionRepository
    {
        public ApplicantAdmissionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
