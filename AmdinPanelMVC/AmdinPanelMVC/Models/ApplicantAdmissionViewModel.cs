using Common.Models.DTOs.Admission;
using Common.Models.DTOs.Applicant;

namespace AmdinPanelMVC.Models
{
    public class ApplicantAdmissionViewModel
    {
        public required ApplicantAdmissionDTO ApplicantAdmission { get; set; }
        public required ApplicantInfo ApplicantInfo { get; set; }
    }
}
