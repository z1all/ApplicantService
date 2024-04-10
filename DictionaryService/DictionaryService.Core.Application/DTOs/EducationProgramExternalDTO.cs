namespace DictionaryService.Core.Application.DTOs
{
    public class EducationProgramExternalDTO : BaseExternalDTO
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string Language { get; set; }
        public required string EducationForm { get; set; }
        public required FacultyExternalDTO Faculty { get; set; }
        public required EducationLevelExternalDTO EducationLevel { get; set; }
    }
}
