﻿using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    public interface IAdmissionProgramRepository : IBaseRepository<AdmissionProgram>
    {
        Task<List<AdmissionProgram>> GetAllByApplicantIdWithProgramWithLevelAndFacultyAsync(Guid applicantId);
        Task<List<AdmissionProgram>> GetAllByApplicantIdWithProgramWithLevelAsync(Guid applicantId);
        Task<List<AdmissionProgram>> GetAllByAdmissionIdAsync(Guid admissionId);
        // Task<AdmissionProgram?> GetLastPriorityAsync(Guid admissionId);
        // Task<int> CountByAdmissionIdAsync(Guid admissionId);
    }
}
