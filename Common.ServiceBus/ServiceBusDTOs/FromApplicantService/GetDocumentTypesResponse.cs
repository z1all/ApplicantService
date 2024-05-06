using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService
{
    public class GetDocumentTypesResponse
    {
        public required List<EducationDocumentTypeDTO> EducationDocumentTypes { get; set; }
    }
}
