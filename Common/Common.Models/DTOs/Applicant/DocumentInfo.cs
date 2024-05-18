using Common.Models.Enums;

namespace Common.Models.DTOs.Applicant
{
    public class DocumentInfo
    {
        public required Guid Id { get; set; }
        public required DocumentType Type { get; set; }
        public required string? Comments { get; set; }
    }
}
