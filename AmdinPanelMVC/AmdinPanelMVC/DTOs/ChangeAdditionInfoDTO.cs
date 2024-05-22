using Common.API.Attributes;
using Common.Models.Attributes;
using Common.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AmdinPanelMVC.DTOs
{
    public class ChangeAdditionInfoDTO
    {
        public required Guid ApplicantId { get; set; }

        [DateValidation(ErrorMessage = "Дата рождения не может быть позже текущей даты")]
        public required DateOnly Birthday { get; set; }
        public required Gender Gender { get; set; }
        [MinLength(5, ErrorMessage = "Минимальная длина составляет 5 символов")]
        public required string Citizenship { get; set; }
        [MinLength(5, ErrorMessage = "Минимальная длина составляет 5 символов")]
        public required string PhoneNumber { get; set; } 
    }
}
