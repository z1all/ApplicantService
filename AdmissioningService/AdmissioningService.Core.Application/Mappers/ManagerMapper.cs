using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Domain;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class ManagerMapper
    {
        public static ManagerDTO ToManagerDTO(this Manager manager)
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
