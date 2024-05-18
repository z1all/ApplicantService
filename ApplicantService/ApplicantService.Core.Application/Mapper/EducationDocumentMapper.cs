using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Domain;
using Common.Models.Enums;

namespace ApplicantService.Core.Application.Mapper
{
    public static class EducationDocumentMapper
    {
        public static EducationDocumentInfo ToEducationDocumentInfo(this EducationDocument educationDocument)
        {
            return new()
            {
                Id = educationDocument.Id,
                Name = educationDocument.Name,
                EducationDocumentType = new()
                {
                    Id = educationDocument.EducationDocumentType!.Id,
                    Name = educationDocument.EducationDocumentType!.Name,
                },
                Scans = educationDocument.FilesInfo.Select(file => new ScanInfo()
                {
                    Id = file.Id,
                    Name = file.Name,
                    Type = file.Type
                }).ToList()
            };
        }
        
        public static EducationDocument ToEducationDocument(this EditAddEducationDocumentInfo documentInfo, Guid applicantId, string comments)
        {
            return new()
            {
                ApplicantId = applicantId,
                ApplicantIdCache = applicantId,
                DocumentType = DocumentType.EducationDocument,
                EducationDocumentTypeId = documentInfo.EducationDocumentTypeId,
                Name = documentInfo.Name,
                Comments = comments,
            };
        }
    }
}
