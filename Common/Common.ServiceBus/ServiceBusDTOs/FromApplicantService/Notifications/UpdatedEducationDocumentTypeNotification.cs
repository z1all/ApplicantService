using Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Base;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Notifications
{
    public class UpdatedEducationDocumentTypeNotification : BaseEducationDocumentType
    {
        public Guid LastEducationDocumentTypeId { get; set; }
    }
}
