namespace Common.Models.DTOs.Applicant
{
    public class ApplicantInfo
    {
        public required ApplicantProfile ApplicantProfile {  get; set; }
        public required List<DocumentInfo> Documents {  get; set; }
    }
}
