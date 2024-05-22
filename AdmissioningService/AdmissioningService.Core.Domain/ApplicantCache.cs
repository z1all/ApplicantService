using Common.Repositories;

namespace AdmissioningService.Core.Domain
{
    public class ApplicantCache : BaseEntity
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }

        public IEnumerable<ApplicantAdmission> Admissions { get; set; } = null!;

        public IEnumerable<EducationDocumentTypeCache> AddedDocumentTypes { get; set; } = null!;
    }
}
