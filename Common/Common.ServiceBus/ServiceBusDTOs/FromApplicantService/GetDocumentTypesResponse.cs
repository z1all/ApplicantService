using Common.Models.DTOs.Dictionary;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService
{
    public class GetDocumentTypesResponse
    {
        public required List<EducationDocumentTypeDTO> EducationDocumentTypes { get; set; }
    }
}
