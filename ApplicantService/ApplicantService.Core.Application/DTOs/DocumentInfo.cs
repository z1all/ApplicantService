using ApplicantService.Core.Domain.Enums;

namespace ApplicantService.Core.Application.DTOs
{
    public class DocumentInfo
    {
        public required Guid Id { get; set; }
        public required DocumentType Type { get; set; }
    }
}
