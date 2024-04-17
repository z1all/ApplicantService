using Common.DTOs;

namespace Common.ServiceBusDTOs.FromDictionaryService
{
    public class GetEducationLevelsResponse
    {
        public required List<EducationLevelDTO> EducationLevels { get; set; }
    }
}
