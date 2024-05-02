using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Domain;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class ApplicantAdmissionMapper
    {
        public static ApplicantAdmissionDTO ToApplicantAdmissionDTO(this ApplicantAdmission applicantAdmission)
        {
            return new()
            {
                AdmissionCompany = applicantAdmission.AdmissionCompany!.ToAdmissionCompanyDTO(),
                AdmissionPrograms = applicantAdmission.AdmissionPrograms.Select(admissionProgram => admissionProgram.ToAdmissionProgramDTO()).ToList(),
                ExistManager = applicantAdmission.ManagerId is not null,
                LastUpdate = applicantAdmission.LastUpdate
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
