using System.Text.Json.Serialization;

namespace Common.Models.Enums
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

    public static class UpdateStatusEnumExtension
    {
        public static string ToRu(this UpdateStatusEnum status)
        {
            return status switch
            {
                UpdateStatusEnum.Loading => "Загрузка с сервера",
                UpdateStatusEnum.ErrorInLoading => "Ошибка при загрузке",
                UpdateStatusEnum.Updating => "Обновление",
                UpdateStatusEnum.ErrorInUpdating => "Ошибка при обновлении",
                UpdateStatusEnum.Updated => "Обновлен",
                UpdateStatusEnum.Wait => "Ожидание",
                _ => ((int)status).ToString(),
            };
        }
    }
}
