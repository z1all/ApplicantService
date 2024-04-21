using ApplicantService.Core.Application.Validation;

namespace ApplicantService.Core.Application.DTOs
{
    public class EditAddPassportInfo
    {
        public required string SeriesNumber { get; set; }
        public required string BirthPlace { get; set; }
        [DateValidation]
        public required DateOnly IssueYear { get; set; }
        public required string IssuedByWhom { get; set; }
    }
}
