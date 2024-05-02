using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Application.Mappers;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications;
using Common.Models.Models;
using Common.ServiceBus.NotificationSender;
using EasyNetQ;

namespace DictionaryService.Core.Application.Services
{
    public class EasyNetQNotificationService : NotificationSender, INotificationService
    {
        public EasyNetQNotificationService(IBus bus) : base(bus) { }

        public async Task<ExecutionResult> AddedEducationDocumentTypeAndEducationLevelAsync(List<EducationDocumentType> documentTypes, List<EducationLevel> levels)
        {
            bool result = await SendingHandler(new EducationLevelAndEducationDocumentTypeAddedNotification()
            {
                EducationLevels = levels.Select(level => level.ToEducationLevelDTO()).ToList(),
                EducationDocumentTypes = documentTypes.Select(documentType => documentType.ToEducationDocumentTypeDTO()).ToList(),
            });

            return GiveResult(result, "An error occurred when sending a notification about education levels and education document types added.");
        }

        public async Task<ExecutionResult> ChangedEducationDocumentTypeAsync(EducationDocumentType documentType)
        {
            bool result = await SendingHandler(new EducationDocumentTypeUpdatedNotification()
            {
                EducationDocumentType = documentType.ToEducationDocumentTypeDTO(),
                Deprecated = documentType.Deprecated,
            });

            return GiveResult(result, "An error occurred when sending a notification about education document type updated.");
        }

        public async Task<ExecutionResult> ChangedEducationLevelAsync(EducationLevel educationLevel)
        {
            bool result = await SendingHandler(new EducationLevelUpdatedNotification()
            {
                EducationLevel = educationLevel.ToEducationLevelDTO(),
                Deprecated = educationLevel.Deprecated,
            });

            return GiveResult(result, "An error occurred when sending a notification about education level updated.");
        }

        public async Task<ExecutionResult> ChangedEducationProgramAsync(EducationProgram program)
        {
            bool result = await SendingHandler(new EducationProgramUpdatedNotification()
            {
                EducationProgram = program.ToEducationProgramDTO(),
                Deprecated = program.Deprecated,
            });

            return GiveResult(result, "An error occurred when sending a notification about education program updated.");
        }

        public async Task<ExecutionResult> ChangedFacultiesAsync(Faculty faculty)
        {
            bool result = await SendingHandler(new FacultyUpdatedNotification()
            {
                Faculty = faculty.ToFacultyDTO(),
                Deprecated = faculty.Deprecated,
            });

            return GiveResult(result, "An error occurred when sending a notification about faculty updated.");
        }
    }
}
