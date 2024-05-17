namespace Common.Models.DTOs.Dictionary
{
    public class EducationDocumentTypeDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required EducationLevelDTO EducationLevel { get; set; }
        public required List<EducationLevelDTO> NextEducationLevel { get; set; }
    }
}