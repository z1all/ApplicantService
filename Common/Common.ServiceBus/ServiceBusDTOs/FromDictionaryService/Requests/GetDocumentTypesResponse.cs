using Common.Models.DTOs.Dictionary;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests
{
    public class GetDocumentTypesResponse
    {
        public required List<EducationDocumentTypeDTO> DocumentTypes { get; set; }
    }
}
