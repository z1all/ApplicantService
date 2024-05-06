using AdmissioningService.Core.Domain;
using Common.Models.DTOs;

namespace AdmissioningService.Core.Application.Mappers
{
    public static class FacultyMapper
    {
        public static FacultyCache ToFacultyCache(this FacultyDTO faculty)
        {
            return new()
            {
                Id = faculty.Id,
                Name = faculty.Name,
                Deprecated = false
            };
        }

        public static FacultyDTO ToFacultyDTO(this FacultyCache faculty)
        {
            return new()
            {
                Id = faculty.Id,
                Name = faculty.Name,
            };
        }
    }
}
