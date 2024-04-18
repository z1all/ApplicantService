using System.Text.Json.Serialization;

namespace Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DictionaryType
    {
        Faculty = 1,
        EducationProgram = 2,
        EducationLevel = 3,
        EducationDocumentType = 4,
    }
}
