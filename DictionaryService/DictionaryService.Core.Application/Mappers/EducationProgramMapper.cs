using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Domain;

namespace DictionaryService.Core.Application.Mappers
{
    public static class EducationProgramMapper
    {
        public static EducationProgramDTO ToEducationProgramDTO(this EducationProgram educationProgram)
        {
            return new()
            {
                Id = educationProgram.Id,
                Name = educationProgram.Name,
                Code = educationProgram.Code,
                CreateTime = educationProgram.CreatedTime,
                EducationForm = educationProgram.EducationForm,
                Language = educationProgram.Language,
                EducationLevel = educationProgram.EducationLevel!.ToEducationLevelDTO(),
                Faculty = educationProgram.Faculty!.ToFacultyDTO(),
            };
        }
    }
}
