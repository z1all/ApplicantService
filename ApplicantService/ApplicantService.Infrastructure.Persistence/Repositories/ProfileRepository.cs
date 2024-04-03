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
                .Include(applicant => applicant.User)
                .FirstOrDefaultAsync(applicant => applicant.UserId == id);
        }

        public async Task<bool> AnyByIdAsync(Guid id)
        {
            return await _dbContext.Applicants
                .AsNoTracking()
                .AnyAsync(applicant => applicant.UserId == id);
        }

        public async Task CreateAsync(UserCache user, Applicant applicant)
        {
            await _dbContext.UsersCache.AddAsync(user);
            await _dbContext.Applicants.AddAsync(applicant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Applicant applicant)
        {
            _dbContext.Entry(applicant).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserCache user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
