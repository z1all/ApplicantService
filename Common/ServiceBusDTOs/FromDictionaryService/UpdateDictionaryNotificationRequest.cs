using Common.Enums;

namespace Common.ServiceBusDTOs.FromDictionaryService
{
    public class UpdateDictionaryNotificationRequest 
    {
        public required DictionaryType DictionaryType { get; set; }
    }
}
