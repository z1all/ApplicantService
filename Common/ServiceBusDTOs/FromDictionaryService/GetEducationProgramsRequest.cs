using Common.DTOs;

namespace Common.ServiceBusDTOs.FromDictionaryService
{
    public class GetEducationProgramsRequest
    {
        public required EducationProgramFilterDTO ProgramFilter {  get; set; }
    }
}
