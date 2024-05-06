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

        public async Task<List<AdmissionProgram>> GetAllByApplicantIdAndAdmissionIdWithProgramWithLevelAndFacultyOrderByPriorityAsync(Guid applicantId, Guid admissionId)
        {
            return await _dbContext.AdmissionPrograms
                .Include(admissionProgram => admissionProgram.EducationProgram)
                    .ThenInclude(program => program!.EducationLevel)
                 .Include(admissionProgram => admissionProgram.EducationProgram)
                    .ThenInclude(program => program!.Faculty)
                .Where(admissionProgram => admissionProgram.ApplicantAdmission!.ApplicantId == applicantId && 
                                           admissionProgram.ApplicantAdmissionId == admissionId)
                .OrderBy(admissionProgram => admissionProgram.Priority)
                .ToListAsync();
        }

        public async Task<List<AdmissionProgram>> GetAllByApplicantIdWithProgramWithLevelAsync(Guid applicantId)
        {
            return await _dbContext.AdmissionPrograms
                .Include(admissionProgram => admissionProgram.EducationProgram)
                    .ThenInclude(program => program!.EducationLevel)
                .Where(admissionProgram => admissionProgram.ApplicantAdmission!.ApplicantId == applicantId)
                .ToListAsync();
        }

        public async Task<List<AdmissionProgram>> GetAllByAdmissionIdWithOrderByPriorityAsync(Guid admissionId)
        {
            return await _dbContext.AdmissionPrograms
                .Where(admissionProgram => admissionProgram.ApplicantAdmissionId == admissionId)
                .OrderBy(admissionProgram => admissionProgram.Priority)
                .ToListAsync();
        }

        public async Task UpdateRangeAsync(List<AdmissionProgram> admissionPrograms)
        {
            _dbContext.AdmissionPrograms.UpdateRange(admissionPrograms);
            await _dbContext.SaveChangesAsync();
        }

        //public async Task<AdmissionProgram?> GetByAdmissionIdAndProgramIdAsync(Guid admissionId, Guid programId)
        //{
        //    return await _dbContext.AdmissionPrograms
        //        .FirstOrDefaultAsync(admissionProgram => admissionProgram.ApplicantAdmissionId == admissionId &&
        //                                                 admissionProgram.EducationProgramId == programId);
        //}

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
