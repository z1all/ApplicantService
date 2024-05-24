using Common.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Application.DTOs
{
    public class EditAddPassportInfo
    {
        [MinLength(5)]
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
