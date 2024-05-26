using Common.Models.DTOs.Applicant;

namespace AmdinPanelMVC.Models
{
    public class ScansViewModel
    {
        public required List<ScanInfo> Scans { get; set; }
        public required bool CanEdit { get; set; }
    }
}
