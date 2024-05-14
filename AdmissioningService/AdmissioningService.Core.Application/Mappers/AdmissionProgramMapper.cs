using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Domain;
using Common.Models.DTOs;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class AdmissionProgramMapper
    {
        public static AdmissionProgramDTO ToAdmissionProgramDTO(this AdmissionProgram admission)
        {
            return new()
            {
                Priority = admission.Priority,
                Deprecated = admission.EducationProgram!.Deprecated,
                EducationProgram = admission.EducationProgram!.ToEducationProgramDTO(),
            };
        }

        public static AdmissionProgramShortInfoDTO ToAdmissionProgramShortInfoDTO(this AdmissionProgram program)
        {
            return new AdmissionProgramShortInfoDTO()
            {
                Priority = program.Priority,
                EducationProgram = program.EducationProgram!.ToEducationProgramShortInfoDTO(),
            };
        }
    }
}
