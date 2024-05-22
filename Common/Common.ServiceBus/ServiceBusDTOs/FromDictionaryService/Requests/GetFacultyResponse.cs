using Common.Models.DTOs.Dictionary;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests
{
    public class GetFacultyResponse
    {
        public required FacultyDTO Faculty { get; set; }
    }
}
