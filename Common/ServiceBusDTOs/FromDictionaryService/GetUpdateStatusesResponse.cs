using Common.DTOs;

namespace Common.ServiceBusDTOs.FromDictionaryService
{
    public class GetUpdateStatusesResponse
    {
        public List<UpdateStatusDTO> UpdateStatuses { get; set; } = null!;
    }
}
