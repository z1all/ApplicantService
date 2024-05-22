using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class UpdateAccessRequestDTO
    {
        [Required]
        public string Refresh { get; set; } = null!;
    }
}
