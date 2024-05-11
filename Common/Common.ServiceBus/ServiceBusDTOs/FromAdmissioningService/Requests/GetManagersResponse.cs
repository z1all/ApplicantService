﻿using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class GetManagersResponse
    {
        public required List<ManagerProfileDTO> Managers { get; set; }
    }
}
