using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Domain
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
