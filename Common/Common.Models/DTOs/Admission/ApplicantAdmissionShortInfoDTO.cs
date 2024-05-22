using Common.Models.Enums;

namespace Common.Models.DTOs.Admission
{
    public class ApplicantAdmissionShortInfoDTO
    {
        public required Guid Id { get; set; }
        public required DateTime LastUpdate { get; set; }
        public required bool ExistManager { get; set; }
        public required AdmissionStatus AdmissionStatus { get; set; }
        public required UserDTO Applicant { get; set; }
        public required List<AdmissionProgramShortInfoDTO> AdmissionPrograms { get; set; }
    }
}
