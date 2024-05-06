using Microsoft.EntityFrameworkCore;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class AdmissionCompanyRepository : BaseWithBaseEntityRepository<AdmissionCompany, AppDbContext>, IAdmissionCompanyRepository
    {
        public AdmissionCompanyRepository(AppDbContext dbContext) : base(dbContext) { }

        public Task<List<AdmissionCompany>> GetAllWithApplicantAdmissionAsync(Guid applicantId)
        {
            return _dbContext.AdmissionCompanies
                .Include(admissionCompanies => admissionCompanies.ApplicantAdmissions
                                                .Where(admission => admission.ApplicantId == applicantId))
                .ToListAsync();
        }

        public async Task<AdmissionCompany?> GetCurrentAsync()
        {
            return await _dbContext.AdmissionCompanies
                .FirstOrDefaultAsync(admissionCompany => admissionCompany.IsCurrent);
        }
    }
}
