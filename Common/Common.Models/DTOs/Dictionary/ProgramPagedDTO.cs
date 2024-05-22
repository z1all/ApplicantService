namespace Common.Models.DTOs.Dictionary
{
    public class ProgramPagedDTO
    {
        public required List<EducationProgramDTO> Programs { get; set; }
        public required PageInfoDTO Pagination { get; set; }
    }
}
