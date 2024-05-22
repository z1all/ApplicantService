using ApplicantService.Core.Application.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class GetEducationDocumentResponse
    {
        public required EducationDocumentInfo EducationDocument { get; set; }
    }
}
