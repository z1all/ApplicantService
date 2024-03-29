namespace ApplicantService.Core.Domain
{
    public class UserCache
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
    }
}
