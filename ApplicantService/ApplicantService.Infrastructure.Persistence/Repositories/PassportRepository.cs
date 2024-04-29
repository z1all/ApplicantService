using Microsoft.EntityFrameworkCore;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using ApplicantService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class PassportRepository : BaseWithBaseEntityRepository<Passport, AppDbContext>, IPassportRepository
    {
        public PassportRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Passport?> GetByApplicantIdAsync(Guid applicantId)
        {
            return await _dbContext.Passports
                .Include(passport => passport.FilesInfo)
                .FirstOrDefaultAsync(passport => passport.ApplicantId == applicantId);
        }

        public async Task<bool> AnyByApplicantIdAsync(Guid applicantId)
        {
            return await _dbContext.Passports
                .AnyAsync(passport => passport.ApplicantId == applicantId);
        }
    }
}
