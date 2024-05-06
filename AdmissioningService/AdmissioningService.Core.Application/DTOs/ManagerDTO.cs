using Common.Models.DTOs;

namespace AdmissioningService.Core.Application.DTOs
{
    public class ManagerDTO
    {
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required FacultyDTO? Faculty { get; set; }
    }
}
