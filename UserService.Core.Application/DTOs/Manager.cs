namespace UserService.Core.Application.DTOs
{
    public class Manager
    {
        public Guid Id { get; set; }
        public string FullName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public Guid? FacultyId { get; init; } = null;
    }
}
