using System.Text.Json.Serialization;

namespace Common.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AdmissionStatus
    {
        Created = 0,
        UnderConsideration = 1,
        Confirmed = 2,
        Rejected = 3,
        Closed = 4,
    }
}
