using ApplicantService.Core.Domain.Enums;

namespace ApplicantService.Core.Application.DTOs
{
    public class ApplicantProfile
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly Birthday { get; set; }
        public Gender Gender { get; set; }
        public string Citizenship { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}