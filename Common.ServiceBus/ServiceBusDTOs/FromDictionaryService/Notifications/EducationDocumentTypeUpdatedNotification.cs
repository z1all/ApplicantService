using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications
{
    public class EducationDocumentTypeUpdatedNotification
    {
        public required EducationDocumentTypeDTO EducationDocumentType { get; set; }
        //public required Guid Id { get; set; }
        //public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
