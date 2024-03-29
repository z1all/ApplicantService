using ApplicantService.Core.Domain.Enums;

namespace ApplicantService.Core.Domain
{
    public class Document
    {
        public required int Id { get; set; }
        public required DocumentType DocumentType { get; set; }

        public required Guid ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }

        public IEnumerable<FileInfo> FilesInfo { get; set; } = Enumerable.Empty<FileInfo>();
    }
}
