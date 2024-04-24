using AdmissioningService.Core.Domain;
using Common.Models.DTOs;

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
    }
}
