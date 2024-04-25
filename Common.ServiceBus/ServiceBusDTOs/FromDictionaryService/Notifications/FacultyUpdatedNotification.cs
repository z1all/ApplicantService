using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications
{
    public class FacultyUpdatedNotification
    {
        public required FacultyDTO Faculty { get; set; }
        //public required Guid Id { get; set; }
        //public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
