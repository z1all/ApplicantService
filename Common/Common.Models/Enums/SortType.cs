using System.Text.Json.Serialization;

namespace Common.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortType
    {
        None = 0,
        LastUpdateAsc = 1,
        LastUpdateDesc = 2,
    }
}
