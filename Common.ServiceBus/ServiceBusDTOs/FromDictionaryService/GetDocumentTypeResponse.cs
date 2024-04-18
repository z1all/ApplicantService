using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService
{
    public class GetDocumentTypeResponse
    {
        public required List<EducationDocumentTypeDTO> DocumentTypes { get; set; }
    }
}
