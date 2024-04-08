using Common.ServiceBusDTOs.FromApplicantService.Base;

namespace Common.ServiceBusDTOs.FromApplicantService
{
    public class UpdatedEducationDocumentTypeNotification : BaseEducationDocumentType
    {
        public Guid LastEducationDocumentTypeId { get; set; }
    }
}
