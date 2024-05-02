using AdmissioningService.Core.Domain;
using Common.Models.Enums;

namespace AdmissioningService.Core.Application.Interfaces.StateMachines
{
    public interface IApplicantAdmissionStateMachin
    {
        Task CreateApplicantAdmissionAsync(Guid applicantId, AdmissionCompany admissionCompany);
        Task AddManagerAsync(ApplicantAdmission applicantAdmission, Manager manager);
        Task DeleteManagerAsync(ApplicantAdmission applicantAdmission);
        Task ApplicantInfoUpdatedAsync(Guid applicantId);
        Task AddAdmissionProgramAsync(AdmissionProgram admissionProgram);
        Task DeleteAdmissionProgramAsync(AdmissionProgram admissionProgram);
        Task UpdateAdmissionProgramRangeAsync(List<AdmissionProgram> admissionPrograms);
        Task ChangeAdmissionStatusAsync(ApplicantAdmission applicantAdmission, ManagerChangeAdmissionStatus changeAdmissionStatus);
    }
}
