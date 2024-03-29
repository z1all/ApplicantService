using System.ComponentModel.DataAnnotations;

namespace UserService.Core.Application.DTOs
{
    public class ChangeProfileRequest
    {
        [Required]
        public string NewFullName { get; set; } = null!;
    }
}
