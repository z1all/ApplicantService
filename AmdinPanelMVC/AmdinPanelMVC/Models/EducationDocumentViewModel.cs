using Common.Models.DTOs.Applicant;

namespace AmdinPanelMVC.Models
{
    public class EducationDocumentViewModel
    {
        public required Guid ApplicantId { get; set; }
        public required EducationDocumentInfo EducationDocument { get; set; }
    }
}
