﻿using Common.Models.DTOs.Dictionary;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests
{
    public class GetUpdateStatusesResponse
    {
        public List<UpdateStatusDTO> UpdateStatuses { get; set; } = null!;
    }
}
