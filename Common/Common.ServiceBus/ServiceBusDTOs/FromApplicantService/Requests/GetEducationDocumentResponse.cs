using Common.Models.DTOs.Applicant;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class GetEducationDocumentResponse
    {
        public required EducationDocumentInfo EducationDocument { get; set; }
    }
}
