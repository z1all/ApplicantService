using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class UpdateAccessRequest
    {
        [Required]
        public string Refresh { get; set; } = null!;
    }
}
