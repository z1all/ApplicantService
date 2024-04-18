using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService
{
    public class GetEducationProgramsRequest
    {
        public required EducationProgramFilterDTO ProgramFilter { get; set; }
    }
}
