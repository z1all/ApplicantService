using Common.Models.DTOs.Applicant;

namespace AmdinPanelMVC.Models
{
    public class PassportViewModel
    {
        public required Guid ApplicantId { get; set; }
        public required PassportInfo Passport { get; set; }
        public required bool CanEdit { get; set; }
    }
}
