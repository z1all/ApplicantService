using System.ComponentModel.DataAnnotations;
using Common.Models.Attributes;

namespace AmdinPanelMVC.DTOs
{
    public class ChangePassportDTO
    {
        public required Guid ApplicantId { get; set; }
        public required string SeriesNumber { get; set; }
        [MinLength(5, ErrorMessage = "Минимальная длина составляет 5 символов")]
        public required string BirthPlace { get; set; }
        [DateValidation(ErrorMessage = "Дата выдачи не может быть позже текущей даты")]
        public required DateOnly IssueYear { get; set; }
        [MinLength(5, ErrorMessage = "Минимальная длина составляет 5 символов")]
        public required string IssuedByWhom { get; set; }
    }
}
