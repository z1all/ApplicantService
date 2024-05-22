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

    public static class ManagerChangeAdmissionStatusExtension
    {
        public static string ToRu(this ManagerChangeAdmissionStatus status)
        {
            return status switch
            {
                ManagerChangeAdmissionStatus.Confirmed => "Подтверждено",
                ManagerChangeAdmissionStatus.Rejected => "Отклонено",
                ManagerChangeAdmissionStatus.Closed => "Закрыто",
                _ => throw new InvalidDataException(),
            };
        }
    }
}
