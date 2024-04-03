using ApplicantService.Core.Domain.Enums;

namespace ApplicantService.Core.Domain
{
    public class Applicant
    {
        public DateOnly? Birthday { get; set; }
        public Gender? Gender { get; set; }
        public string? Citizenship { get; set; }
        public string? PhoneNumber { get; set; }

        public required Guid UserId { get; set; }
        public UserCache? User { get; set; }

        public IEnumerable<Document> Documents { get; set; } = null!;
    }
}
