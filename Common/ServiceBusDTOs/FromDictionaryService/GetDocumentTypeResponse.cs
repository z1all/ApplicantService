using Common.DTOs;

namespace Common.ServiceBusDTOs.FromDictionaryService
{
    public class GetDocumentTypeResponse
    {
        public required List<EducationDocumentTypeDTO> DocumentTypes { get; set; }
    }
}
