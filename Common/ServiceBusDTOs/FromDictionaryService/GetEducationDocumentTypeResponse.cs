namespace Common.ServiceBusDTOs.FromDictionaryService
{
    public class GetEducationDocumentTypeResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
