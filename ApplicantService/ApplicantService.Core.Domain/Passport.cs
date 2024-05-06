namespace ApplicantService.Core.Domain
{
    public class Passport : Document
    {
        public required string SeriesNumber { get; set; }
        public required string BirthPlace { get; set; }
        public required DateOnly IssueYear { get; set; }
        public required string IssuedByWhom { get; set; }

        public required Guid ApplicantIdCache { get; set; }
    }
}