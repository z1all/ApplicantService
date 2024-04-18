using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService
{
    public class GetEducationLevelsResponse
    {
        public required List<EducationLevelDTO> EducationLevels { get; set; }
    }
}
