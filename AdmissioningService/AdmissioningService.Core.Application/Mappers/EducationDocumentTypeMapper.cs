using AdmissioningService.Core.Domain;
using Common.Models.DTOs.Dictionary;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class EducationDocumentTypeMapper
    {
        public static EducationDocumentTypeCache ToEducationDocumentTypeCache(this EducationDocumentTypeDTO documentType, List<EducationLevelCache> educationLevelsCache)
        {
            var nextEducationLevels = documentType.NextEducationLevel.Select(nextEducationLevel =>
                educationLevelsCache.First(educationLevel => educationLevel.Id == nextEducationLevel.Id))
                .ToList();

            return new()
            {
                Id = documentType.Id,
                Name = documentType.Name,
                EducationLevelId = documentType.EducationLevel.Id,
                NextEducationLevel = nextEducationLevels,
                Deprecated = false,
            };
        }
    }
}
