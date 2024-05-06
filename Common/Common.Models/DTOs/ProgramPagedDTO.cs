namespace Common.Models.DTOs
{
    public class ProgramPagedDTO
    {
        public required List<EducationProgramDTO> Programs { get; set; }
        public required PageInfoDTO Pagination { get; set; }
    }
}
