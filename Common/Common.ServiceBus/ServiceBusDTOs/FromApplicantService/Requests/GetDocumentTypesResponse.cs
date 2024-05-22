using Common.Models.DTOs.Dictionary;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class GetDocumentTypesResponse
    {
        public required List<EducationDocumentTypeDTO> EducationDocumentTypes { get; set; }
    }
}
