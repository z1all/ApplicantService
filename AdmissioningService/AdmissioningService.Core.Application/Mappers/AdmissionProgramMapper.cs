using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Domain;

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
    }
}
