using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Domain;
using Common.Models;
using Common.ServiceBusDTOs.FromDictionaryService;
using EasyNetQ;

namespace DictionaryService.Core.Application.Services
{
    public class EasyNetQNotificationService : INotificationService
    {
        private readonly IBus _bus;

        public EasyNetQNotificationService(IBus bus) 
        {
            _bus = bus;
        }

        public async Task<ExecutionResult> ChangedEducationDocumentTypeAsync(EducationDocumentType documentType)
        {
            bool result = await SendingHandler(new EducationDocumentTypeUpdatedNotification()
            {
                Id = documentType.Id,
                Name = documentType.Name,
                Deprecated = documentType.Deprecated,
            });

            return GiveResult(result, "An error occurred when sending a notification about education document type updated.");
        }

        public async Task<ExecutionResult> ChangedEducationLevelAsync(EducationLevel educationLevel)
        {
            bool result = await SendingHandler(new EducationLevelUpdatedNotification()
            {
                Id = educationLevel.Id,
                Name = educationLevel.Name,
                Deprecated = educationLevel.Deprecated,
            });

            return GiveResult(result, "An error occurred when sending a notification about education level updated.");
        }

        public async Task<ExecutionResult> ChangedEducationProgramAsync(EducationProgram program)
        {
            bool result = await SendingHandler(new EducationProgramUpdatedNotification()
            {
                Id = program.Id,
                Name = program.Name,
                Code = program.Code,
                EducationForm = program.EducationForm,
                Language = program.Language,
                EducationLevelId = program.EducationLevelId,
                FacultyId = program.FacultyId,
                Deprecated = program.Deprecated,
            });

            return GiveResult(result, "An error occurred when sending a notification about education program updated.");
        }

        public async Task<ExecutionResult> ChangedFacultiesAsync(Faculty faculty)
        {
            bool result = await SendingHandler(new FacultyUpdatedNotification()
            {
                Id = faculty.Id,
                Name = faculty.Name,
                Deprecated = faculty.Deprecated,
            });

            return GiveResult(result, "An error occurred when sending a notification about faculty updated.");
        }

        private ExecutionResult GiveResult(bool result, string errorMassage)
        {
            if (!result)
            {
                return new("SendNotificationFail", errorMassage);
            }
            return new(isSuccess: true);
        }

        private async Task<bool> SendingHandler<T>(T notification) where T : class
        {
            return await _bus.PubSub
                .PublishAsync(notification)
                .ContinueWith(task => task.IsCompletedSuccessfully);
        }
    }
}
