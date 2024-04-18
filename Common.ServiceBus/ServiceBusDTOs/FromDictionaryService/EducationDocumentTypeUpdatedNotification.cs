namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService
{
    public class EducationDocumentTypeUpdatedNotification
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
