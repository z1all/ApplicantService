using Common.Repositories;
using DictionaryService.Core.Domain.Enum;

namespace DictionaryService.Core.Domain
{
    public class UpdateStatus : BaseEntity
    {
        public DateTime LastUpdate { get; set; }
        public DictionaryType DictionaryType { get; set; }
        public UpdateStatusEnum Status { get; set; }
    }
}
