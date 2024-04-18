namespace DictionaryService.Infrastructure.ExternalService.Configurations
{
    public class WebExternalOptions
    {
        public required string BaseUrl { get; set; }
        public required string AuthorizationHeader { get; set; }
        public required string EducationLevelRoute { get; set; }
        public required string DocumentTypeRoute { get; set; }
        public required string FacultiesRoute { get; set; }
        public required string EducationProgramRoute { get; set; }
    }
}
