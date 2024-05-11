﻿using Common.Models.DTOs;

namespace AmdinPanelMVC.Models
{
    public class ProfileViewModel
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required FacultyDTO? Faculty { get; set; }
    }
}