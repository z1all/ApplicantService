﻿using Common.Models.DTOs;

namespace AdmissioningService.Core.Application.DTOs
{
    public class EducationProgramShortInfoDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required FacultyDTO Faculty { get; set; }
    }
}
