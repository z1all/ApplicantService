﻿using Common.Models.DTOs.Dictionary;

namespace Common.Models.DTOs.Admission
{
    public class ManagerProfileDTO
    {
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required FacultyDTO? Faculty { get; set; }
    }
}
