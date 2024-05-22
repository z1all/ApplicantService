using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using ApplicantService.Infrastructure.Persistence.Contexts;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class ApplicantRepository : BaseWithBaseEntityRepository<Applicant, AppDbContext>, IApplicantRepository
    {
        public ApplicantRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<Applicant?> GetByIdWithDocumentsAsync(Guid applicantId)
        {
            return await _dbContext.Applicants
                .Include(applicant => applicant.Documents)
                .FirstOrDefaultAsync(applicant => applicant.Id == applicantId);
        }
    }
}
