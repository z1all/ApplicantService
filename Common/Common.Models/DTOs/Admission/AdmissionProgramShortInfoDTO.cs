using Common.Models.DTOs.Dictionary;

namespace Common.Models.DTOs.Admission
{
    public class AdmissionProgramShortInfoDTO
    {
        public required int Priority { get; set; }
        public required EducationProgramShortInfoDTO EducationProgram { get; set; }
    }
}
