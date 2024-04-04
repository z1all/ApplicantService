namespace ApplicantService.Core.Application.DTOs
{
    public class EditAddPassportInfo
    {
        public required string SeriesNumber { get; set; }
        public required string BirthPlace { get; set; }
        public required DateOnly IssueYear { get; set; }
        public required string IssuedByWhom { get; set; }
    }
}
