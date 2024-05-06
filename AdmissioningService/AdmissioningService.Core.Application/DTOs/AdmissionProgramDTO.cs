using Common.Models.DTOs;

namespace AdmissioningService.Core.Application.DTOs
{
    public class AdmissionProgramDTO
    {
        public required int Priority { get; set; }
        public required bool Deprecated { get; set; }
        public required EducationProgramDTO EducationProgram { get; set; }
    }
}
