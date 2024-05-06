using AdmissioningService.Core.Domain;
using Common.Repositories;

namespace AdmissioningService.Core.Application.Interfaces.Repositories
{
    public interface IAdmissionProgramRepository : IBaseRepository<AdmissionProgram>
    {
        [Obsolete("Don't use this repository, use IApplicantAdmissionStateMachin")]
        new Task AddAsync(AdmissionProgram entity);
        [Obsolete("Don't use this repository, use IApplicantAdmissionStateMachin")]
        new Task UpdateAsync(AdmissionProgram entity);
        [Obsolete("Don't use this repository, use IApplicantAdmissionStateMachin")]
        new Task DeleteAsync(AdmissionProgram entity);
        [Obsolete("Don't use this repository, use IApplicantAdmissionStateMachin")]
        new Task SaveChangesAsync();

        [Obsolete("Don't use this repository, use IApplicantAdmissionStateMachin")]
        Task UpdateRangeAsync(List<AdmissionProgram> admissionPrograms);

        Task<List<AdmissionProgram>> GetAllByApplicantIdAndAdmissionIdWithProgramWithLevelAndFacultyOrderByPriorityAsync(Guid applicantId, Guid admissionId);
        Task<List<AdmissionProgram>> GetAllByApplicantIdWithProgramWithLevelAsync(Guid applicantId);
        Task<List<AdmissionProgram>> GetAllByAdmissionIdWithOrderByPriorityAsync(Guid admissionId);
    }
}
