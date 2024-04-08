using Common.Repositories;

namespace DictionaryService.Core.Domain
{
    public class EducationLevel : BaseEntity
    {
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
