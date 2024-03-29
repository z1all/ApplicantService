using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Domain
{
    public class UserCache
    {
        [Key]
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }

        public Applicant? Applicant { get; set; }
    }
}
