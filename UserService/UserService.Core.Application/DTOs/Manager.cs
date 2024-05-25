using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class Manager
    {
        public required Guid Id { get; set; }
        [MinLength(5)]
        public required string FullName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required Guid? FacultyId { get; set; }
    }
}
