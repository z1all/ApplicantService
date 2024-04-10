using Common.Repositories;

namespace DictionaryService.Core.Domain
{
    public class BaseDictionaryEntity : BaseEntity
    {
        public required bool Deprecated { get; set; }
    }
}
