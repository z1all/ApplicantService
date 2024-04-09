using DictionaryService.Core.Domain.Enum;

namespace DictionaryService.Core.Application.DTOs
{
    public class UpdateStatusDTO
    {
        public DateTime? LastUpdate { get; set; }
        public DictionaryType DictionaryType { get; set; }
        public UpdateStatusEnum Status { get; set; }
    }
}
