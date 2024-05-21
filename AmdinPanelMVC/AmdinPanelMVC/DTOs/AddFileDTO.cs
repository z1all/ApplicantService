using Common.API.Attributes;

namespace AmdinPanelMVC.DTOs
{
    public class AddFileDTO
    {
        [FileMaxSizeValidation(10 * 1024 * 1024)]
        [FileAllowTypesValidation]
        public required IFormFile File { get; set; }
    }
}
