using Microsoft.EntityFrameworkCore;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    public class FacultyRepository : BaseWithBaseEntityRepository<Faculty, AppDbContext>, IFacultyRepository
    {
        public FacultyRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<List<Faculty>> GetAllAsync(bool getDeprecated)
        {
            return await _dbContext.Faculties
                .Where(faculty => getDeprecated ? true : !faculty.Deprecated)
                .ToListAsync();
        }

        public override async Task<Faculty?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Faculties
                .FirstOrDefaultAsync(Faculty => Faculty.Id == id);
        }
    }
}
