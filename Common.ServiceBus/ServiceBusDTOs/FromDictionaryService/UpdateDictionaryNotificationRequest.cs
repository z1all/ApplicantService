using Common.Models.Enums;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService
{
    public class UpdateDictionaryNotificationRequest
    {
        public required DictionaryType DictionaryType { get; set; }
    }
}
