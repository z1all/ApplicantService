using Common.Models.Attributes;
using Common.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Application.DTOs
{
    public class EditApplicantProfile
    {
        [DateValidation]
        [Required]
        public required DateOnly Birthday { get; set; }
        [Required]
        public required Gender Gender { get; set; }
        [MinLength(5)]
        public required string Citizenship { get; set; }
        [MinLength(5)]
        public required string PhoneNumber { get; set; }
    }
}
