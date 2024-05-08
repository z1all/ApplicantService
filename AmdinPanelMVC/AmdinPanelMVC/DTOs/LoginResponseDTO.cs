namespace AmdinPanelMVC.DTOs
{
    public class LoginResponseDTO
    {
        public required Guid Id { get; set; }
        public required string FullName { get; init; }
        public required string Email { get; init; }
    }
}
