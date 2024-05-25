using System.ComponentModel.DataAnnotations;
using Common.API.Attributes;
using Common.Models.Attributes;

namespace AmdinPanelMVC.DTOs
{
    public class ChangePassportDTO
    {
        public required Guid ApplicantId { get; set; }
        [PassportNumberValidation(ErrorMessage = "Серия и номер паспорта должны содержать 4 и 6 цифр соответственно разделенные пробелом")]
        public required string SeriesNumber { get; set; }
        [MinLength(5, ErrorMessage = "Минимальная длина составляет 5 символов")]
        public required string BirthPlace { get; set; }
        [DateValidation(ErrorMessage = "Дата выдачи не может быть позже текущей даты")]
        public required DateOnly IssueYear { get; set; }
        [MinLength(5, ErrorMessage = "Минимальная длина составляет 5 символов")]
        public required string IssuedByWhom { get; set; }
    }
}
