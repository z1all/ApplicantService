using Common.Models.DTOs;

namespace DictionaryService.Core.Application.DTOs
{
    public class ProgramListExternalDTO
    {
        public required List<EducationProgramExternalDTO> Programs { get; set; }
        public required PageInfoDTO Pagination { get; set; }
    }
}
