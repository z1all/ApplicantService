using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using ApplicantService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class ApplicantRepository : BaseWithBaseEntityRepository<Applicant, AppDbContext>, IApplicantRepository
    {
        public ApplicantRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
