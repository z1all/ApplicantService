using ApplicantService.Core.Domain.Enums;
using Common.Repositories;

namespace ApplicantService.Core.Domain
{
    public class Document : BaseEntity
    {
        public required DocumentType DocumentType { get; set; }

        public required Guid ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }

        public IEnumerable<DocumentFileInfo> FilesInfo { get; set; } = null!;
    }
}
