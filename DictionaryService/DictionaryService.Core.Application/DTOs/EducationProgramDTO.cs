namespace DictionaryService.Core.Application.DTOs
{
    public class EducationProgramDTO
    {
        public required Guid Id { get; set; }
        public required DateTime CreateTime { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string Language { get; set; }
        public required string EducationForm { get; set; }
        public required FacultyDTO Faculty { get; set; }
        public required EducationLevelDTO EducationLevel { get; set; }
    }
}
