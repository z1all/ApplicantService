namespace DictionaryService.Core.Application.DTOs
{
    public class PageInfoDTO
    {
        public required int Size { get; set; }
        public required int Count { get; set; }
        public required int Current { get; set; }
    }
}
