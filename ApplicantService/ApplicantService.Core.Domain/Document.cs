using ApplicantService.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Domain
{
    public class Document
    {
        [Key]
        public required Guid Id { get; set; } 
        public required DocumentType DocumentType { get; set; }

        public required Guid ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }

        public required IEnumerable<DocumentFileInfo> FilesInfo { get; set; } = Enumerable.Empty<DocumentFileInfo>();
    }
}
