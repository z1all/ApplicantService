using ApplicantService.Core.Domain.Enums;

namespace ApplicantService.Core.Domain
{
    public class Applicant
    {
        public required DateOnly Birthday { get; set; }
        public required Gender Gender { get; set; }
        public required string Citizenship { get; set; } = null!;
        public required string PhoneNumber { get; set; } = null!;

        public required Guid UserId { get; set; }
        public UserCache? User { get; set; }

        public IEnumerable<Document> Documents { get; set; } = Enumerable.Empty<Document>();
    }
}
