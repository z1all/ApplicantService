using System.ComponentModel.DataAnnotations;

namespace Common.Repositories
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
