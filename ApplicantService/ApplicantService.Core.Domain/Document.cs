using ApplicantService.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Domain
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; } 
        public DocumentType DocumentType { get; set; }

        public Guid ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }

        public IEnumerable<DocumentFileInfo> FilesInfo { get; set; } = null!;
    }
}
