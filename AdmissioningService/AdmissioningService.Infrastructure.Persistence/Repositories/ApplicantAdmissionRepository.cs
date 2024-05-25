using Microsoft.EntityFrameworkCore;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;
using AdmissioningService.Infrastructure.Persistence.Contexts;
using Common.Repositories;
using Common.Models.Enums;
using Common.Models.DTOs.Admission;

namespace AdmissioningService.Infrastructure.Persistence.Repositories
{
    internal class ApplicantAdmissionRepository : BaseWithBaseEntityRepository<ApplicantAdmission, AppDbContext>, IApplicantAdmissionRepository
    {
        public ApplicantAdmissionRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<ApplicantAdmission?> GetByIdWithApplicantAsync(Guid admissionId)
        {
            return await _dbContext.ApplicantAdmissions
                .Include(applicantAdmission => applicantAdmission.Applicant)
                .FirstOrDefaultAsync(applicantAdmission => applicantAdmission.Id == admissionId);
        }

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

        public async Task<ApplicantAdmission?> GetCurrentByApplicantIdWithManagerAsync(Guid applicantId)
        {
            return await _dbContext.ApplicantAdmissions
                .Include(applicantAdmissions => applicantAdmissions.Manager)
                .FirstOrDefaultAsync(applicantAdmission => applicantAdmission.ApplicantId == applicantId &&
                                                           applicantAdmission.AdmissionCompany!.IsCurrent);
        }

        public async Task<List<ApplicantAdmission>> GetAllByFiltersWithCompanyAndProgramsAsync(ApplicantAdmissionFilterDTO filter, Guid managerId)
        {
            string? fullName = filter.ApplicantFullName?.ToLower() ?? null;
            string? codeOrNameProgram = filter.CodeOrNameProgram?.ToLower() ?? null;

            var filtered = _dbContext.ApplicantAdmissions
                .Include(admission => admission.Applicant)
                .Include(admission => admission.AdmissionPrograms)
                    .ThenInclude(admissionProgram => admissionProgram.EducationProgram)
                        .ThenInclude(educationProgram => educationProgram!.Faculty)
                .Where(admission =>
                    (admission.AdmissionCompany!.IsCurrent) &&
                    (fullName != null ? admission.Applicant!.FullName.ToLower().Contains(fullName) : true) &&
                    (codeOrNameProgram != null ? admission.AdmissionPrograms
                                                    .Any(program => program.EducationProgram!.Code.Contains(codeOrNameProgram) || 
                                                                    program.EducationProgram!.Name.ToLower().Contains(codeOrNameProgram)) : true) &&
                    (filter.FacultiesId != null ? admission.AdmissionPrograms
                                                    .Any(program => filter.FacultiesId.Contains(program.EducationProgram!.FacultyId)) : true) &&
                    (filter.AdmissionStatus != null ? admission.AdmissionStatus == filter.AdmissionStatus : true) &&
                    (filter.ViewApplicantMode == ViewApplicantMode.OnlyTakenApplicant ? admission.ManagerId == managerId : true) &&
                    (filter.ViewApplicantMode == ViewApplicantMode.OnlyWithoutManager ? admission.ManagerId == null : true));

            var filteredAndSorted = filter.SortType switch
            {
                SortType.LastUpdateAsc => filtered.OrderBy(admission => admission.LastUpdate),
                SortType.LastUpdateDesc => filtered.OrderByDescending(admission => admission.LastUpdate),
                _ => filtered,
            };

            return await filteredAndSorted               
                .Skip((filter.Page - 1) * filter.Size)
                .Take(filter.Size)
                .ToListAsync();
        }

        public async Task<int> CountAllAsync(ApplicantAdmissionFilterDTO filter, Guid managerId)
        {
            string? fullName = filter.ApplicantFullName?.ToLower() ?? null;
            string? codeOrNameProgram = filter.CodeOrNameProgram?.ToLower() ?? null;

            return await _dbContext.ApplicantAdmissions
                .CountAsync(admission =>
                    (admission.AdmissionCompany!.IsCurrent) &&
                    (fullName != null ? admission.Applicant!.FullName.ToLower().Contains(fullName) : true) &&
                    (codeOrNameProgram != null ? admission.AdmissionPrograms
                                                    .Any(program => program.EducationProgram!.Code.Contains(codeOrNameProgram) || 
                                                                    program.EducationProgram!.Name.ToLower().Contains(codeOrNameProgram)) : true) &&
                    (filter.FacultiesId != null ? admission.AdmissionPrograms
                                                    .Any(program => filter.FacultiesId.Contains(program.EducationProgram!.FacultyId)) : true) &&
                    (filter.AdmissionStatus != null ? admission.AdmissionStatus == filter.AdmissionStatus : true) &&
                    (filter.ViewApplicantMode == ViewApplicantMode.OnlyTakenApplicant ? admission.ManagerId == managerId : true) &&
                    (filter.ViewApplicantMode == ViewApplicantMode.OnlyWithoutManager ? admission.ManagerId == null : true));
        }

        public async Task UpdateRangeAsync(List<ApplicantAdmission> entities)
        {
            _dbContext.ApplicantAdmissions.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ApplicantAdmission>> GetAllByManagerIdAsync(Guid managerId)
        {
            return await _dbContext.ApplicantAdmissions
                .Where(admission => admission.ManagerId == managerId)
                .ToListAsync();
        }
    }
}
