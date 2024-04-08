using Common.Repositories;

namespace DictionaryService.Core.Domain
{
    public class Faculty : BaseEntity
    {
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }

        public IEnumerable<EducationProgram> Programs { get; set; } = null!;
    }
}
