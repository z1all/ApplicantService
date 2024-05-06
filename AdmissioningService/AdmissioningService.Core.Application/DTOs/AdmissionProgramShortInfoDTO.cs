namespace AdmissioningService.Core.Application.DTOs
{
    public class AdmissionProgramShortInfoDTO
    {
        public required int Priority { get; set; }
        public required EducationProgramShortInfoDTO EducationProgram { get; set; }
    }
}
