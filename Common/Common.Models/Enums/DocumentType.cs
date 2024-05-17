using System.Text.Json.Serialization;

namespace Common.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DocumentType
    {
        Passport = 0,
        EducationDocument = 1,
    }
}
