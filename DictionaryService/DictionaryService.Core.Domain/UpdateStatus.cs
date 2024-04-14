using DictionaryService.Core.Domain.Enum;
using Common.Repositories;

namespace DictionaryService.Core.Domain
{
    public class UpdateStatus : BaseEntity
    {
        public DateTime? LastUpdate { get; set; }
        public required DictionaryType DictionaryType { get; set; }
        public required UpdateStatusEnum Status { get; set; }
        public string? Comments { get; set; } 
    }
}
