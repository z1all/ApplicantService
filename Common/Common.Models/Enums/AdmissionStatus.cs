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

    public static class AdmissionStatusExtension
    {
        public static string ToRu(this AdmissionStatus status)
        {
            return status switch
            {
                AdmissionStatus.Created => "Создано",
                AdmissionStatus.UnderConsideration => "На рассмотрении",
                AdmissionStatus.Confirmed => "Подтверждено",
                AdmissionStatus.Rejected => "Отклонено",
                AdmissionStatus.Closed => "Закрыто",
                _ => throw new InvalidDataException(),
            };
        }
    }
}
