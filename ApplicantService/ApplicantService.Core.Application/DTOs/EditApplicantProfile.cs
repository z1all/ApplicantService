using ApplicantService.Core.Application.Validation;
using ApplicantService.Core.Domain.Enums;

namespace ApplicantService.Core.Application.DTOs
{
    public class EditApplicantProfile
    {
        [DateValidation]
        public required DateOnly Birthday { get; set; }
        public required Gender Gender { get; set; }
        public required string Citizenship { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
