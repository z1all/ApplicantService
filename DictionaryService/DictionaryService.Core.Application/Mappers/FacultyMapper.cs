using Common.Models.DTOs;
using DictionaryService.Core.Domain;

namespace DictionaryService.Core.Application.Mappers
{
    public static class FacultyMapper
    {
        public static FacultyDTO ToFacultyDTO(this Faculty faculty)
        {
            return new()
            {
                Id = faculty.Id,
                Name = faculty.Name,
            };
        }
    }
}
