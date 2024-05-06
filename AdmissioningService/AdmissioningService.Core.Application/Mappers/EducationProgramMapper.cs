using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Domain;
using Common.Models.DTOs;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class EducationProgramMapper
    {
        public static EducationProgramDTO ToEducationProgramDTO(this EducationProgramCache educationProgram)
        {
            return new()
            {
                Id = educationProgram.Id,
                Code = educationProgram.Code,
                Name = educationProgram.Name,
                Language = educationProgram.Language,
                EducationForm = educationProgram.EducationForm,
                EducationLevel = educationProgram.EducationLevel!.ToEducationLevelDTO(),
                Faculty = educationProgram.Faculty!.ToFacultyDTO(),
            };
        }

        public static EducationProgramShortInfoDTO ToEducationProgramShortInfoDTO(this EducationProgramCache educationProgram)
        {
            return new()
            {
                Id = educationProgram.Id,
                Name = educationProgram.Name,
                Code = educationProgram.Code,
                Faculty = educationProgram.Faculty!.ToFacultyDTO()
            };
        }
    }
}
