namespace ApplicantService.Core.Domain
{
    public class DocumentFileInfo : BaseEntity
    {
        public required string PathName { get; set; }

        public required Guid DocumentId { get; set; }
        public Document? Document { get; set; }
    }
}
