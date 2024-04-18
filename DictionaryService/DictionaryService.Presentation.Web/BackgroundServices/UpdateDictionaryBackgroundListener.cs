using DictionaryService.Core.Application.Interfaces.Services;
using Common.ServiceBusDTOs.FromDictionaryService;
using EasyNetQ.AutoSubscribe;

namespace DictionaryService.Presentation.Web.BackgroundServices
{
    public class UpdateDictionaryBackgroundListener : IConsumeAsync<UpdateDictionaryNotificationRequest>, IConsumeAsync<UpdateAllDictionaryNotificationRequest>
    {
        private readonly IUpdateDictionaryService _updateDictionaryService;

        public UpdateDictionaryBackgroundListener(IUpdateDictionaryService updateDictionaryService) 
        {
            _updateDictionaryService = updateDictionaryService;
        }

        public async Task ConsumeAsync(UpdateDictionaryNotificationRequest message, CancellationToken cancellationToken = default)
        {
            await _updateDictionaryService.UpdateDictionaryAsync(message.DictionaryType);
        }

        public async Task ConsumeAsync(UpdateAllDictionaryNotificationRequest message, CancellationToken cancellationToken = default)
        {
            await _updateDictionaryService.UpdateAllDictionaryAsync();
        }
    }
}
