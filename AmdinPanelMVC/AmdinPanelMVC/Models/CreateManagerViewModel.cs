using System.Diagnostics.CodeAnalysis;

namespace AmdinPanelMVC.Models
{
    public class CreateManagerViewModel
    {
        public required Guid? Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string? Password { get; set; }
        public Guid? FacultyId { get; set; } 
    }
}
