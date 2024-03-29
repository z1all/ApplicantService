using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Domain
{
    public class DocumentFileInfo
    {
        [Key]
        public required Guid Id { get; set; }
        public required string PathName { get; set; }

        public required Guid DocumentId { get; set; }
        public Document? Document { get; set; }
    }
}
