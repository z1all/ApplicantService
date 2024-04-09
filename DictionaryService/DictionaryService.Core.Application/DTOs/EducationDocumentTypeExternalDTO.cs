namespace DictionaryService.Core.Application.DTOs
{
    public class EducationDocumentTypeExternalDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required EducationLevelExternalDTO EducationLevel { get; set; }
        public required List<EducationLevelExternalDTO> NextEducationLevels { get; set; }
    }
}
