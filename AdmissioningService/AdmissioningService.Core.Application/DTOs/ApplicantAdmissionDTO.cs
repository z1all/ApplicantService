using Common.Models.Enums;

namespace AdmissioningService.Core.Application.DTOs
{
    public class ApplicantAdmissionDTO
    {
        public required DateTime LastUpdate { get; set; }
        public required bool ExistManager { get; set; }
        public required AdmissionStatus AdmissionStatus { get; set; }
        public required AdmissionCompanyDTO AdmissionCompany { get; set; }
        public required List<AdmissionProgramDTO> AdmissionPrograms { get; set; }
    }
}
