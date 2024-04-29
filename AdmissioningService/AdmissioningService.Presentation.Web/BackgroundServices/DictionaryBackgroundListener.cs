using AdmissioningService.Core.Application.Interfaces.Services;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications;
using EasyNetQ.AutoSubscribe;

namespace AdmissioningService.Presentation.Web.BackgroundServices
{
    public class DictionaryBackgroundListener : 
        IConsumeAsync<EducationLevelAndEducationDocumentTypeAddedNotification>,
        IConsumeAsync<EducationDocumentTypeUpdatedNotification>,
        IConsumeAsync<EducationLevelUpdatedNotification>,
        IConsumeAsync<EducationProgramUpdatedNotification>,
        IConsumeAsync<FacultyUpdatedNotification>
    {
        private readonly IDictionaryBackgroundService _dictionaryBackgroundService;

        public DictionaryBackgroundListener(IDictionaryBackgroundService dictionaryBackgroundService) 
        {
            _dictionaryBackgroundService = dictionaryBackgroundService;
        }

        public async Task ConsumeAsync(EducationLevelAndEducationDocumentTypeAddedNotification message, CancellationToken cancellationToken = default)
        {
            await _dictionaryBackgroundService.AddEducationLevelAndEducationDocumentTypeAsync(message.EducationLevels, message.EducationDocumentTypes);
        }

        public async Task ConsumeAsync(EducationDocumentTypeUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            await _dictionaryBackgroundService.UpdateEducationDocumentTypeAsync(message.EducationDocumentType, message.Deprecated);
        }

        public async Task ConsumeAsync(EducationLevelUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            await _dictionaryBackgroundService.UpdateEducationLevelAsync(message.EducationLevel, message.Deprecated);
        }

        public async Task ConsumeAsync(EducationProgramUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            await _dictionaryBackgroundService.UpdateEducationProgramAsync(message.EducationProgram, message.Deprecated);
        }

        public async Task ConsumeAsync(FacultyUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            await _dictionaryBackgroundService.UpdateFacultyAsync(message.Faculty, message.Deprecated);
        }
    }
}
