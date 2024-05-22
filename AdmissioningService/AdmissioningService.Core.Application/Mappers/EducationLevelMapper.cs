using AdmissioningService.Core.Domain;
using Common.Models.DTOs.Dictionary;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class EducationLevelMapper
    {
        public static EducationLevelCache ToEducationLevelCache(this EducationLevelDTO educationLevel)
        {
            return new()
            {
                Id = educationLevel.Id,
                Name = educationLevel.Name,
                Deprecated = false,
            };
        }

        public static EducationLevelDTO ToEducationLevelDTO(this EducationLevelCache educationLevel)
        {
            return new()
            {
                Id = educationLevel.Id,
                Name = educationLevel.Name,
            };
        }
    }
}
