namespace Common.Models.DTOs.Dictionary
{
    public class EducationProgramFilterDTO
    {
        public string? FacultyName { get; set; }
        public List<Guid>? EducationLevel { get; set; }
        public string? EducationForm { get; set; }
        public string? Language { get; set; }
        public string? CodeOrNameProgram { get; set; }

        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
