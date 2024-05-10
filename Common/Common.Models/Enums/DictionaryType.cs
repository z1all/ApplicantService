using System.Text.Json.Serialization;

namespace Common.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DictionaryType
    {
        Faculty = 1,
        EducationProgram = 2,
        EducationLevel = 3,
        EducationDocumentType = 4,
    }

    public static class DictionaryTypeExtension
    {
        public static string ToRu(this DictionaryType type)
        {
            return type switch
            {
                DictionaryType.Faculty => "Факультеты",
                DictionaryType.EducationProgram => "Программы образования",
                DictionaryType.EducationLevel => "Уровни образования",
                DictionaryType.EducationDocumentType => "Типы документов об образовании",
            };
        }
    }

}
