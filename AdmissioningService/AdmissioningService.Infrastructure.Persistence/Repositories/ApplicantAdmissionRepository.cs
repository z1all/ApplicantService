using Microsoft.EntityFrameworkCore;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    [Obsolete]
    internal class ApplicantAdmissionRepository : BaseWithBaseEntityRepository<ApplicantAdmission, AppDbContext>, IApplicantAdmissionRepository
    {
        public ApplicantAdmissionRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<ApplicantAdmission?> GetByAdmissionCompanyId(Guid admissionCompanyId)
        {
            return await _dbContext.ApplicantAdmissions
                .FirstOrDefaultAsync(applicantAdmission => applicantAdmission.AdmissionCompanyId == admissionCompanyId);
        }

        public async Task<ApplicantAdmission?> GetByApplicantIdAndAdmissionIdAsync(Guid applicantId, Guid admissionId)
        {
            return await _dbContext.ApplicantAdmissions
                .Include(applicantAdmissions => applicantAdmissions.AdmissionCompany)
                .FirstOrDefaultAsync(applicantAdmissions => applicantAdmissions.ApplicantId == applicantId && applicantAdmissions.Id == admissionId);
        }

        public async Task<bool> AnyByApplicantIdAndAdmissionIdAsync(Guid applicantId, Guid admissionId)
        {
            return await _dbContext.ApplicantAdmissions
               .AnyAsync(applicantAdmissions => applicantAdmissions.ApplicantId == applicantId && applicantAdmissions.Id == admissionId);
        }
    }
}
