﻿using Common.Models.DTOs.Admission;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class GetManagerProfileResponse
    {
        public required ManagerProfileDTO Manager { get; set; }
    }
}
