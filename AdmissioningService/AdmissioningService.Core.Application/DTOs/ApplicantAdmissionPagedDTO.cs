using Common.Models.DTOs;

namespace AdmissioningService.Core.Application.DTOs
{
    public class ApplicantAdmissionPagedDTO
    {
        public required List<ApplicantAdmissionShortInfoDTO> ApplicantAdmissions { get; set; }
        public required PageInfoDTO Pagination { get; set; }
    }
}
