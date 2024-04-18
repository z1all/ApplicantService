using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService
{
    public class GetUpdateStatusesResponse
    {
        public List<UpdateStatusDTO> UpdateStatuses { get; set; } = null!;
    }
}
