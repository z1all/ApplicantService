namespace Common.Models.DTOs.Admission
{
    public class ApplicantAdmissionPagedDTO
    {
        public required List<ApplicantAdmissionShortInfoDTO> ApplicantAdmissions { get; set; }
        public required PageInfoDTO Pagination { get; set; }
    }
}
