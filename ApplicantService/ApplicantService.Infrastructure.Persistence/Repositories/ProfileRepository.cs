using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using ApplicantService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ApplicantService.Infrastructure.Persistence.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _dbContext;

        public ProfileRepository(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<Applicant?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Applicants
                .AsNoTracking()
                .FirstOrDefaultAsync(applicant => applicant.UserId == id);
        }

        public async Task UpdateAsync(Applicant applicant)
        {
            _dbContext.Entry(applicant).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
