using Common.Models.DTOs.Dictionary;

namespace Common.Models.DTOs.Admission
{
    public class AdmissionProgramDTO
    {
        public required int Priority { get; set; }
        public required bool Deprecated { get; set; }
        public required EducationProgramDTO EducationProgram { get; set; }
    }
}
