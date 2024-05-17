using Common.Models.Enums;

namespace Common.Models.DTOs.Applicant
{
    public class ApplicantProfile
    {
        public required Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly? Birthday { get; set; }
        public Gender? Gender { get; set; }
        public string? Citizenship { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
    }
}