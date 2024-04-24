using Common.Models.Enums;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications
{
    public class UpdateDictionaryNotificationRequest
    {
        public required DictionaryType DictionaryType { get; set; }
    }
}
