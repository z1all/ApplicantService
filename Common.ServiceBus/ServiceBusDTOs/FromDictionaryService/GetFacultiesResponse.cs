using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService
{
    public class GetFacultiesResponse
    {
        public required List<FacultyDTO> Faculties { get; set; } = null!;
    }
}
