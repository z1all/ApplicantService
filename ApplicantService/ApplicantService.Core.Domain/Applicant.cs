using ApplicantService.Core.Domain.Enums;
using Common.Repositories;

namespace ApplicantService.Core.Domain
{
    public class Applicant : BaseEntity
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public DateOnly? Birthday { get; set; }
        public Gender? Gender { get; set; }
        public string? Citizenship { get; set; }
        public string? PhoneNumber { get; set; }

        public IEnumerable<Document> Documents { get; set; } = null!;
    }
}
