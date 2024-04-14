using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Core.Application.DTOs
{
    public class EducationProgramFilterDTO
    {
        public string? FacultyName { get; set; }
        public List<Guid>? EducationLevel { get; set; }
        public string? EducationForm { get; set; }
        public string? Language { get; set; }
        public string? CodeOrNameProgram { get; set; }

        [Required]
        public required int Page {  get; set; }
        [Required]
        public required int Size {  get; set; }
    }
}
