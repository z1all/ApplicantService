using System.Text.Json.Serialization;

namespace Common.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ViewApplicantMode
    {
        All = 0,
        OnlyWithoutManager = 1,
        OnlyTakenApplicant = 2,
    }
}
