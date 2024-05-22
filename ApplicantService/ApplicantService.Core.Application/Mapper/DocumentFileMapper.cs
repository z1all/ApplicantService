using ApplicantService.Core.Domain;
using Common.Models.DTOs.Applicant;

namespace ApplicantService.Core.Application.Mapper
{
    public static class DocumentFileMapper
    {
        public static FileDTO ToFileDTO(this FileEntity fileEntity, DocumentFileInfo documentFileInfo)
        {
            return new()
            {
                Name = documentFileInfo.Name,
                Type = documentFileInfo.Type,
                File = fileEntity.File,
            };
        }

        public static (FileEntity, DocumentFileInfo) ToFileEntityAndDocumentFileInfo(this FileDTO fileDTO, Guid documentId)
        {
            FileEntity fileEntity = new()
            {
                File = fileDTO.File
            };

            DocumentFileInfo documentFileInfo = new()
            {
                DocumentId = documentId,
                Name = fileDTO.Name,
                Type = fileDTO.Type,
            };

            return (fileEntity, documentFileInfo);
        }

        public static ScanInfo ToScanInfo(this DocumentFileInfo fileInfo)
        {
            return new()
            {
                Id = fileInfo.Id,
                Name = fileInfo.Name,
                Type = fileInfo.Type,
            };
        }
    }
}
