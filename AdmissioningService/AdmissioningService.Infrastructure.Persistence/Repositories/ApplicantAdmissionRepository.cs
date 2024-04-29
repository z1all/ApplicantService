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

        public async Task<ApplicantAdmission?> GetByAdmissionCompanyIdAndApplicantId(Guid admissionCompanyId, Guid applicantId)
        {
            return await _dbContext.ApplicantAdmissions
                .FirstOrDefaultAsync(applicantAdmission => applicantAdmission.AdmissionCompanyId == admissionCompanyId && 
                                                           applicantAdmission.ApplicantId == applicantId);
        }

        public async Task<ApplicantAdmission?> GetByApplicantIdAndAdmissionIdAsync(Guid applicantId, Guid admissionId)
        {
            return await _dbContext.ApplicantAdmissions
                .Include(applicantAdmissions => applicantAdmissions.AdmissionCompany)
                .FirstOrDefaultAsync(applicantAdmissions => applicantAdmissions.ApplicantId == applicantId && 
                                                            applicantAdmissions.Id == admissionId);
        }

        public async Task<ApplicantAdmission?> GetCurrentByApplicantIdAsync(Guid applicantId)
        {
            return await _dbContext.ApplicantAdmissions
                .FirstOrDefaultAsync(applicantAdmission => applicantAdmission.ApplicantId == applicantId &&
                                                           applicantAdmission.AdmissionCompany!.IsCurrent);
        }
    }
}
