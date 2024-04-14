namespace DictionaryService.Core.Domain
{
    public class Faculty : BaseDictionaryEntity
    { 
        public IEnumerable<EducationProgram> EducationPrograms { get; set; } = null!;
    }
}
