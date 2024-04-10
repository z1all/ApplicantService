namespace DictionaryService.Core.Application.DTOs
{
    public class ProgramListExternalDTO : BaseExternalDTO
    {
        public required List<EducationProgramExternalDTO> Programs { get; set; }
    }
}
