using Common.Models.Enums;

namespace Common.Models.DTOs.Admission
{
    public class ApplicantAdmissionDTO
    {
        public required Guid Id { get; set; }
        public required DateTime LastUpdate { get; set; }
        public required Guid? ManagerId { get; set; }
        public required AdmissionStatus AdmissionStatus { get; set; }
        public required AdmissionCompanyDTO AdmissionCompany { get; set; }
        public required List<AdmissionProgramDTO> AdmissionPrograms { get; set; }
    }
}
