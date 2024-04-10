namespace DictionaryService.Core.Domain
{
    public class Faculty : BaseDictionaryEntity
    {
        public required string Name { get; set; }

        public IEnumerable<EducationProgram> EducationPrograms { get; set; } = null!;
    }
}
