using Common.Models.Enums;

namespace AmdinPanelMVC.Models
{
    public class AdmissionsFilterViewModel
    {
        public string? ApplicantFullName { get; set; }
        public string? CodeOrNameProgram { get; set; }
        public List<Guid>? FacultiesId { get; set; }
        public AdmissionStatus? AdmissionStatus { get; set; }
        public ViewApplicantMode ViewApplicantMode { get; set; } = ViewApplicantMode.All;
        public SortType SortType { get; set; } = SortType.None;

        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
