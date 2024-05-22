using AdmissioningService.Core.Domain;
using Common.Models.DTOs.Admission;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class ManagerMapper
    {
        public static ManagerProfileDTO ToManagerDTO(this Manager manager)
        {
            return new()
            {
                Id = manager.User!.Id,
                Email = manager.User!.Email,
                FullName = manager.User!.FullName,
                Faculty = manager.Faculty?.ToFacultyDTO(),
            };
        }
    }
}
