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

        public Task<ExecutionResult> ChangedEducationLevelAsync(EducationLevel educationLevel)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> ChangedEducationProgramAsync(EducationProgram program)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> ChangedFacultiesAsync(Faculty faculty)
        {
            throw new NotImplementedException();
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
