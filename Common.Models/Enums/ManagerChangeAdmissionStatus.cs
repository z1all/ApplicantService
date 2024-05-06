using System.Text.Json.Serialization;

namespace Common.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ManagerChangeAdmissionStatus
    {
        Confirmed = 2,
        Rejected = 3,
        Closed = 4,
    }
}
