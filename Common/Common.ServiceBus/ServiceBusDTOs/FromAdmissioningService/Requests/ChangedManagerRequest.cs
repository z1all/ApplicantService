﻿using Common.Models.DTOs.Admission;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class ChangedManagerRequest
    {
        public required Guid ManagerId { get; set; }
        public required ManagerDTO Manager { get; set; }
    }
}
