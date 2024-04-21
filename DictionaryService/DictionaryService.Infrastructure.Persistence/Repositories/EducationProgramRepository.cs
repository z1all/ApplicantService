using Microsoft.EntityFrameworkCore;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Common.Repositories;
using Common.Models.DTOs;

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

        public async Task<List<EducationProgram>> GetAllAsync()
        {
            return await _dbContext.EducationPrograms.ToListAsync();
        }

        public override async Task<EducationProgram?> GetByIdAsync(Guid id)
        {
            return await _dbContext.EducationPrograms
                .FirstOrDefaultAsync(Faculty => Faculty.Id == id);
        }

        public async Task<List<EducationProgram>> GetAllByFiltersAsync(EducationProgramFilterDTO filter, bool getDeprecated)
        {
            return await _dbContext.EducationPrograms
                .Include(educationProgram => educationProgram.Faculty)
                .Include(educationProgram => educationProgram.EducationLevel)
                .Where(educationProgram => (filter.FacultyName != null ? educationProgram.Faculty!.Name.ToLower().Contains(filter.FacultyName.ToLower()) : true) &&
                                           (filter.EducationForm != null ? educationProgram.EducationForm.ToLower().Contains(filter.EducationForm.ToLower()) : true) &&
                                           (filter.Language != null ? educationProgram.Language.ToLower().Contains(filter.Language.ToLower()) : true) && 
                                           (filter.CodeOrNameProgram != null ? educationProgram.Code.Contains(filter.CodeOrNameProgram.ToLower()) 
                                                                            || educationProgram.Name.Contains(filter.CodeOrNameProgram.ToLower()) : true) &&
                                           (filter.EducationLevel != null ? filter.EducationLevel.Contains(educationProgram.EducationLevel!.Id) : true) &&
                                           (getDeprecated ? true : !educationProgram.Deprecated && !educationProgram.EducationLevel!.Deprecated && !educationProgram.Faculty!.Deprecated)) 
                .Skip((filter.Page - 1) * filter.Size)
                .Take(filter.Size)
                .ToListAsync();
        }

        public async Task<int> GetAllCountAsync(EducationProgramFilterDTO filter, bool getDeprecated)
        {
            return await _dbContext.EducationPrograms
                .CountAsync(educationProgram => (filter.FacultyName != null ? educationProgram.Faculty!.Name.ToLower().Contains(filter.FacultyName.ToLower()) : true) &&
                                           (filter.EducationForm != null ? educationProgram.EducationForm.ToLower().Contains(filter.EducationForm.ToLower()) : true) &&
                                           (filter.Language != null ? educationProgram.Language.ToLower().Contains(filter.Language.ToLower()) : true) &&
                                           (filter.CodeOrNameProgram != null ? educationProgram.Code.Contains(filter.CodeOrNameProgram.ToLower())
                                                                            || educationProgram.Name.Contains(filter.CodeOrNameProgram.ToLower()) : true) &&
                                           (filter.EducationLevel != null ? filter.EducationLevel.Contains(educationProgram.EducationLevel!.Id) : true) &&
                                           (getDeprecated ? true : !educationProgram.Deprecated && !educationProgram.EducationLevel!.Deprecated && !educationProgram.Faculty!.Deprecated));
        }
    }
}
