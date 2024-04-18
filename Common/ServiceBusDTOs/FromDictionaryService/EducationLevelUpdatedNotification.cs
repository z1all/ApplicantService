namespace Common.ServiceBusDTOs.FromDictionaryService
{
    public class EducationLevelUpdatedNotification
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
