using System.Text.Json.Serialization;

namespace ApplicantService.Core.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DocumentType
    {
        Passport = 0, 
        EducationDocument = 1,
    }
}
