﻿namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService
{
    public class EducationProgramUpdatedNotification
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }

        public required string Code { get; set; }
        public required string Language { get; set; }
        public required string EducationForm { get; set; }

        public required Guid EducationLevelId { get; set; }
        public required Guid FacultyId { get; set; }
    }
}