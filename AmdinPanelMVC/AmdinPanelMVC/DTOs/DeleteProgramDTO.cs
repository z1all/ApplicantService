namespace AmdinPanelMVC.DTOs
{
    public class DeleteProgramDTO
    {
        public required Guid ApplicantId { get; set; }
        public required Guid ProgramId { get; set; }
    }
}
