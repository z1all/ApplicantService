using System.Text.Json.Serialization;

namespace ApplicantService.Core.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        male = 0, 
        female = 1,
    }
}
