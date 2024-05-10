namespace Common.Models.DTOs
{
    public class ManagerDTO
    {
        //public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required Guid? FacultyId { get; set; }
    }
}
