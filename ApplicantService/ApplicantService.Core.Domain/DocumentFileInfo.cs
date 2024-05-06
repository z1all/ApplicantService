using Common.Repositories;

namespace ApplicantService.Core.Domain
{
    public class DocumentFileInfo : BaseEntity
    {
        public string PathName { get => $"{Id}_{Name}{Type}"; }
        public required string Name { get; set; }
        public required string Type { get; set; }

        public required Guid DocumentId { get; set; }
        public Document? Document { get; set; }
    }
}
