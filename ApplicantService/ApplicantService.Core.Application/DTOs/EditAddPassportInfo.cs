using System.ComponentModel.DataAnnotations;
using Common.Models.Attributes;

namespace ApplicantService.Core.Application.DTOs
{
    public class EditAddPassportInfo
    {
        [PassportNumberValidation(ErrorMessage = "The passport series and number must contain 4 and 6 digits, respectively, separated by a space")]
        public required string SeriesNumber { get; set; }
        [MinLength(5)]
        public required string BirthPlace { get; set; }
        [DateValidation]
        [Required]
        public required DateOnly IssueYear { get; set; }
        [MinLength(5)]
        public required string IssuedByWhom { get; set; }
    }
}
