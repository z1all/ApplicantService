using Common.Models.Attributes;

namespace Common.Models.DTOs.Applicant
{
    public class PassportInfo
    {
        public required Guid Id { get; set; }
        public required string SeriesNumber { get; set; }
        public required string BirthPlace { get; set; }
        [DateValidation]
        public required DateOnly IssueYear { get; set; }
        public required string IssuedByWhom { get; set; }
        public required List<ScanInfo> Scans { get; set; }
    }
}
