using Common.DTOs;

namespace Common.ServiceBusDTOs.FromDictionaryService
{
    public class GetFacultiesResponse
    {
        public required List<FacultyDTO> Faculties { get; set; } = null!;
    }
}
