using Common.Models.DTOs.Dictionary;
using DictionaryService.Core.Domain;

namespace DictionaryService.Core.Application.Mappers
{
    public static class DocumentTypeMapper
    {
        public static EducationDocumentTypeDTO ToEducationDocumentTypeDTO(this EducationDocumentType documentType)
        {
            return new()
            {
                Id = documentType.Id,
                Name = documentType.Name,
                EducationLevel = documentType.EducationLevel!.ToEducationLevelDTO(),
                NextEducationLevel = documentType.NextEducationLevels
                    .Select(educationLevel => educationLevel.ToEducationLevelDTO()).ToList(),
            };
        }
    }
}
