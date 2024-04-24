namespace AdmissioningService.Core.Application.DTOs
{
    public class ChangePrioritiesApplicantProgramDTO
    {
        public required List<Guid> NewProgramPrioritiesOrder { get; set; }
    }
}
