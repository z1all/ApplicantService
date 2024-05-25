using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class ChangeProfileRequestDTO
    {
        [Required]
        [MinLength(5)]
        public string NewFullName { get; set; } = null!;
    }
}
