using Microsoft.EntityFrameworkCore;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class AdmissionProgramRepository : BaseRepository<AdmissionProgram, AppDbContext>, IAdmissionProgramRepository
    {
        public AdmissionProgramRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<AdmissionProgram>> GetAllByApplicantIdWithProgramAndLevelAndFacultyAsync(Guid applicantId)
        {
            return await _dbContext.AdmissionPrograms
                .Include(admissionProgram => admissionProgram.EducationProgram)
                    .ThenInclude(program => program!.EducationLevel)
                 .Include(admissionProgram => admissionProgram.EducationProgram)
                    .ThenInclude(program => program!.Faculty)
                .Where(admissionProgram => admissionProgram.ApplicantAdmission!.ApplicantId == applicantId)
                .ToListAsync();
        }

        public async Task<List<AdmissionProgram>> GetAllByAdmissionIdAsync(Guid admissionId)
        {
            return await _dbContext.AdmissionPrograms
                .Where(admissionProgram => admissionProgram.ApplicantAdmissionId == admissionId)
                .ToListAsync();
        }

        //public async Task<AdmissionProgram?> GetLastPriorityAsync(Guid admissionId)
        //{
        //    return await _dbContext.AdmissionPrograms
        //        .Where(admissionProgram => admissionProgram.ApplicantAdmissionId == admissionId)
        //        .OrderByDescending(admissionProgram => admissionProgram.Priority)
        //        .FirstOrDefaultAsync();
        //}

        //public async Task<int> CountByAdmissionIdAsync(Guid admissionId)
        //{
        //    return await _dbContext.AdmissionPrograms
        //        .CountAsync(admissionProgram => admissionProgram.ApplicantAdmissionId == admissionId);
        //}
    }
}
