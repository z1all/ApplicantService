using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests
{
    public class GetEducationLevelsResponse
    {
        public required List<EducationLevelDTO> EducationLevels { get; set; }
    }
}
