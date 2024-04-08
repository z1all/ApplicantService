namespace ApplicantService.Core.Application.DTOs
{
    public class PassportInfo
    {
        public required Guid Id { get; set; }
        public required string SeriesNumber { get; set; }
        public required string BirthPlace { get; set; }
        public required DateOnly IssueYear { get; set; }
        public required string IssuedByWhom { get; set; }
        public required List<ScanInfo> Scans { get; set; }
    }
}
