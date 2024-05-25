using System.ComponentModel.DataAnnotations;

namespace AmdinPanelMVC.Models
{
    public class ChangeFullNameViewModel
    {
        [MinLength(5, ErrorMessage = "Имя должно иметь длину не менее 5 символов")]
        public required string FullName { get; set; }
    }
}
