namespace UserService.Core.Application.DTOs
{
    public class ManagerLoggedinResponse
    {
        public required Guid Id { get; set; }
        public required string FullName { get; init; }
        public required string Email { get; init; }
    }
}
