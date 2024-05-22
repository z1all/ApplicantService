using Common.Models.Enums;

namespace Common.Models.DTOs.Dictionary
{
    public class UpdateStatusDTO
    {
        public required DateTime? LastUpdate { get; set; }
        public required DictionaryType DictionaryType { get; set; }
        public required UpdateStatusEnum Status { get; set; }
        public required string? Comments { get; set; }
    }
}
