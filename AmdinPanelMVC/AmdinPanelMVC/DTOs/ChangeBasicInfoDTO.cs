using System.ComponentModel.DataAnnotations;

namespace AmdinPanelMVC.DTOs
{
    public class ChangeBasicInfoDTO
    {
        [MinLength(5, ErrorMessage = "Минимальная длина ФИО составляет 5 символов")]
        public required string FullName { get; set; }
        public required Guid ApplicantId { get; set; }
    }
}
