using System.Text.Json.Serialization;

namespace LoggerService.Core.Application.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LogSortType
    {
        None = 0,
        DateTimeAsc = 1,
        DateTimeDesc = 2,
    }
}
