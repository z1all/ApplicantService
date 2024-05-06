using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests
{
    public class GetEducationProgramsResponse
    {
        public required ProgramPagedDTO ProgramPagedDTO { get; set; }
    }
}
