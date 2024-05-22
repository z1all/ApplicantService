using Common.Models.DTOs.Dictionary;

namespace Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications
{
    public class EducationLevelAndEducationDocumentTypeAddedNotification
    {
        public required List<EducationLevelDTO> EducationLevels { get; set; }
        public required List<EducationDocumentTypeDTO> EducationDocumentTypes { get; set; }
    }
}
