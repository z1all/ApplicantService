using System.Text.Json.Serialization;

namespace Common.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        male = 0,
        female = 1,
    }
}
