namespace Common.Models.DTOs.Admission
{
    public class AdmissionCompanyDTO
    {
        public required Guid Id { get; set; }
        public required int EventYear { get; set; }
        public required bool IsCurrent { get; set; }
        public required Guid? ApplicantAdmissioningId { get; set; }
    }
}
