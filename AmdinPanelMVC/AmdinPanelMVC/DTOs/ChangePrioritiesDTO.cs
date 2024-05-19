using Common.Models.DTOs.Admission;

namespace AmdinPanelMVC.DTOs
{
    public class ChangePrioritiesDTO
    {
        public required Guid ApplicantId { get; set; }
        public required ChangePrioritiesApplicantProgramDTO NewPriorities { get; set; }
    }
}
