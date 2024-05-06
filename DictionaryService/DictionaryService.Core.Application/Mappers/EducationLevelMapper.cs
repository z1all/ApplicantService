using Common.Models.DTOs;
using DictionaryService.Core.Domain;

namespace DictionaryService.Core.Application.Mappers
{
    public static class EducationLevelMapper
    {
        public static EducationLevelDTO ToEducationLevelDTO(this EducationLevel educationLevel)
        {
            return new()
            {
                Id = educationLevel.Id,
                Name = educationLevel.Name,
            };
        }
    }
}
