using System.Text.Json.Serialization;

namespace Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UpdateStatusEnum
    {
        Loading = 1,
        ErrorInLoading = 2,
        Updating = 3,
        ErrorInUpdating = 4,
        Updated = 5,
        Wait = 6,
    }
}
