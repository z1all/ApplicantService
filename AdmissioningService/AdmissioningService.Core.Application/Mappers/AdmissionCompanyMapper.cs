using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Domain;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class AdmissionCompanyMapper
    {
        public static AdmissionCompanyDTO ToAdmissionCompanyDTO(this AdmissionCompany admissionCompany)
        {
            return new()
            {
                Id = admissionCompany.Id,
                EventYear = admissionCompany.EventYear,
                IsCurrent = admissionCompany.IsCurrent,
                ApplicantAdmissioningId = admissionCompany.ApplicantAdmissions.FirstOrDefault()?.Id ?? null,
            };
        }
    }
}
