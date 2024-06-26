﻿using AdmissioningService.Core.Domain;
using Common.Models.DTOs.Admission;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    public interface IApplicantAdmissionRepository : IBaseWithBaseEntityRepository<ApplicantAdmission>
    {
        [Obsolete("Don't use this method, use IApplicantAdmissionStateMachin")]
        new Task AddAsync(ApplicantAdmission entity);
        [Obsolete("Don't use this method, use IApplicantAdmissionStateMachin")]
        new Task UpdateAsync(ApplicantAdmission entity);
        [Obsolete("Don't use this method, use IApplicantAdmissionStateMachin")]
        Task UpdateRangeAsync(List<ApplicantAdmission> entities);
        [Obsolete("Don't use this method, use IApplicantAdmissionStateMachin")]
        new Task DeleteAsync(ApplicantAdmission entity);
        [Obsolete("Don't use this method, use IApplicantAdmissionStateMachin")]
        new Task SaveChangesAsync();

        Task<ApplicantAdmission?> GetByIdWithApplicantAsync(Guid admissionId);
        Task<ApplicantAdmission?> GetByAdmissionCompanyIdAndApplicantId(Guid admissionCompanyId, Guid applicantId);
        Task<ApplicantAdmission?> GetCurrentByApplicantIdAsync(Guid applicantId);
        Task<ApplicantAdmission?> GetCurrentByApplicantIdWithManagerAsync(Guid applicantId);
        Task<ApplicantAdmission?> GetCurrentByApplicantIdWithProgramsAsync(Guid applicantId);
        Task<ApplicantAdmission?> GetByApplicantIdAndAdmissionIdAsync(Guid applicantId, Guid admissionId);
        Task<int> CountAllAsync(ApplicantAdmissionFilterDTO admissionFilter, Guid managerId);
        Task<List<ApplicantAdmission>> GetAllByFiltersWithCompanyAndProgramsAsync(ApplicantAdmissionFilterDTO admissionFilter, Guid managerId);
        Task<List<ApplicantAdmission>> GetAllByManagerIdAsync(Guid managerId);
    }
}
