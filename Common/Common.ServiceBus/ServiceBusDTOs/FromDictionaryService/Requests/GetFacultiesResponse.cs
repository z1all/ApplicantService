using Common.Models.DTOs.Dictionary;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests
{
    public class GetFacultiesResponse
    {
        public required List<FacultyDTO> Faculties { get; set; } = null!;
    }
}
