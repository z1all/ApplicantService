namespace Common.Models.DTOs.Admission
{
    public class ChangePrioritiesApplicantProgramDTO
    {
        public required List<Guid> NewProgramPrioritiesOrder { get; set; }
    }
}
