using Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Base;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService
{
    public class UpdatedEducationDocumentTypeNotification : BaseEducationDocumentType
    {
        public Guid LastEducationDocumentTypeId { get; set; }
    }
}
