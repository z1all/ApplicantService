using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class ChangeProfileRequestDTO
    {
        [Required]
        public string NewFullName { get; set; } = null!;
    }
}
