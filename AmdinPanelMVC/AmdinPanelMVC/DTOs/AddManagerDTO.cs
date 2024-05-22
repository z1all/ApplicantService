namespace AmdinPanelMVC.DTOs
{
    public class AddManagerDTO
    {
        public required Guid AdmissionId { get; set; }
        public required Guid? ManagerId { get; set; }
    }
}
