﻿namespace Common.Models.DTOs.Applicant
{
    public class EducationDocumentInfo
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required EducationDocumentTypeInfo EducationDocumentType { get; set; }
        public required List<ScanInfo> Scans { get; set; }
    }
}
