using AdmissioningService.Core.Domain;
using Common.Models.DTOs.Admission;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class ApplicantAdmissionMapper
    {
        public static ApplicantAdmissionDTO WithProgramsToApplicantAdmissionDTO(this ApplicantAdmission applicantAdmission)
        {
            return new()
            {
                Id = applicantAdmission.Id,
                LastUpdate = applicantAdmission.LastUpdate,
                ManagerId = applicantAdmission.ManagerId,
                AdmissionStatus = applicantAdmission.AdmissionStatus,
                AdmissionCompany = applicantAdmission.AdmissionCompany!.ToAdmissionCompanyDTO(),
                AdmissionPrograms = applicantAdmission.AdmissionPrograms.Select(admissionProgram => admissionProgram.ToAdmissionProgramDTO()).ToList(),
            };
        }

        public static ApplicantAdmissionDTO ToApplicantAdmissionDTO(this ApplicantAdmission applicantAdmission, List<AdmissionProgram> programs)
        {
            return new()
            {
                Id = applicantAdmission.Id,
                LastUpdate = applicantAdmission.LastUpdate,
                ManagerId = applicantAdmission.ManagerId,
                AdmissionStatus = applicantAdmission.AdmissionStatus,
                AdmissionCompany = applicantAdmission.AdmissionCompany!.ToAdmissionCompanyDTO(),
                AdmissionPrograms = programs.Select(program => program.ToAdmissionProgramDTO()).ToList()
            };
        }

        public static ApplicantAdmissionShortInfoDTO ToApplicantAdmissionShortInfoDTO(this ApplicantAdmission applicantAdmission)
        {
            return new()
            {
                Id = applicantAdmission.Id,
                LastUpdate = applicantAdmission.LastUpdate,
                ExistManager = applicantAdmission.ManagerId is not null,
                AdmissionStatus = applicantAdmission.AdmissionStatus,
                Applicant = new()
                {
                    Id = applicantAdmission.Applicant!.Id,
                    Email = applicantAdmission.Applicant!.Email,
                    FullName = applicantAdmission.Applicant!.FullName,
                },
                AdmissionPrograms = applicantAdmission.AdmissionPrograms.Select(program => program.ToAdmissionProgramShortInfoDTO()).ToList(),
            };
        }
    }
}
