using Common.Models.Enums;

namespace AmdinPanelMVC.Models
{
    public class ChangeAdmissionStatusViewModel
    {
        public required ManagerChangeAdmissionStatus NewStatus { get; set; }
        public required Guid AdmissionId { get; set; }
    }
}
