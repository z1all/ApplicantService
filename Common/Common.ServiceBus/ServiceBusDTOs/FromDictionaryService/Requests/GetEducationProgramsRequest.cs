using Common.Models.DTOs.Dictionary;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests
{
    public class GetEducationProgramsRequest
    {
        public required EducationProgramFilterDTO ProgramFilter { get; set; }
    }
}
