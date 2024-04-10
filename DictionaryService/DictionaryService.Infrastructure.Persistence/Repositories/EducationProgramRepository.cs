using Microsoft.EntityFrameworkCore;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace DictionaryService.Infrastructure.Persistence.Repositories
{
    public class EducationProgramRepository : BaseRepository<EducationProgram, AppDbContext>, IEducationProgramRepository
    {
        public EducationProgramRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<List<EducationProgram>> GetAllByFacultyIdAsync(Guid facultyId)
        {
            return await _dbContext.EducationPrograms
                .Where(educationPrograms => educationPrograms.FacultyId == facultyId)
                .ToListAsync();
        }

        public async Task<List<EducationProgram>> GetAllByEducationLevelIdAsync(Guid educationLevelId)
        {
            return await _dbContext.EducationPrograms
                .Where(educationPrograms => educationPrograms.EducationLevelId == educationLevelId)
                .ToListAsync();
        }
    }
}
