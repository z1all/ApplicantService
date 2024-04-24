using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class ApplicantAdmissionRepository : BaseRepository<ApplicantAdmission, AppDbContext>, IApplicantAdmissionRepository
    {
        public ApplicantAdmissionRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<ApplicantAdmission?> GetByAdmissionCompanyId(Guid admissionCompanyId)
        {
            return await _dbContext.ApplicantAdmissions
                .FirstOrDefaultAsync(applicantAdmission => applicantAdmission.AdmissionCompanyId == admissionCompanyId);
        }
    }
}
