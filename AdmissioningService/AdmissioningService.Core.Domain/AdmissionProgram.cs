namespace AdmissioningService.Core.Domain
{
    public class AdmissionProgram
    {
        public required int Priority { get; set; }

        public required Guid EducationProgramId { get; set; }
        public EducationProgramCache? EducationProgram { get; set; }

        public required Guid ApplicantAdmissionId { get; set; }
        public ApplicantAdmission? ApplicantAdmission { get; set; }
    }
}
