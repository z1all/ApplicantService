using AdmissioningService.Core.Domain;
using Common.Models.DTOs;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class EducationDocumentTypeMapper
    {
        public static EducationDocumentTypeCache ToEducationDocumentTypeCache(this EducationDocumentTypeDTO documentType)
        {
            return new()
            {
                Id = documentType.Id,
                Name = documentType.Name,
                EducationLevelId = documentType.EducationLevel.Id,
                EducationLevel = documentType.EducationLevel.ToEducationLevelCache(),
                NextEducationLevel = documentType.NextEducationLevel
                                        .Select(documentType => documentType.ToEducationLevelCache()),
                Deprecated = false,
            };
        }
    }
}
