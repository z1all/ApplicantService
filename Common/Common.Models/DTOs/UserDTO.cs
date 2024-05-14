namespace Common.Models.DTOs
{
    public class UserDTO
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
    }
}
