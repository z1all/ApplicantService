using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    public interface IAdmissionProgramRepository : IBaseRepository<AdmissionProgram>
    {
        Task<List<AdmissionProgram>> GetAllByApplicantIdAndAdmissionIdWithProgramWithLevelAndFacultyOrderByPriorityAsync(Guid applicantId, Guid admissionId);
        Task<List<AdmissionProgram>> GetAllByApplicantIdWithProgramWithLevelAsync(Guid applicantId);
        Task<List<AdmissionProgram>> GetAllByAdmissionIdWithOrderByPriorityAsync(Guid admissionId);
        // Task<AdmissionProgram?> GetByAdmissionIdAndProgramIdAsync(Guid admissionId, Guid programId);
        Task UpdateRangeAsync(List<AdmissionProgram> admissionPrograms);
        // Task<AdmissionProgram?> GetLastPriorityAsync(Guid admissionId);
        // Task<int> CountByAdmissionIdAsync(Guid admissionId);
    }
}
