namespace Common.Models.DTOs.Admission
{
    public class ManagerDTO
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required Guid? FacultyId { get; set; }
    }
}
