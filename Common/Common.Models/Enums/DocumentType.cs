using System.Text.Json.Serialization;

namespace Common.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DocumentType
    {
        Passport = 0,
        EducationDocument = 1,
    }

    public static class DocumentTypeExtension
    {
        public static string ToRu(this DocumentType type)
        {
            return type switch
            {
                DocumentType.EducationDocument => "Документ об образовании",
                DocumentType.Passport => "Паспорт",
                _ => ((int)type).ToString(),
            };
        }
    }
}
