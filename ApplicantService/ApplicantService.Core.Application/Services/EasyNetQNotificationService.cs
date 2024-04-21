using ApplicantService.Core.Application.Interfaces.Services;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Base;
using EasyNetQ;
using Common.Models.Models;

namespace ApplicantService.Core.Application.Services
{
    public class EasyNetQNotificationService : INotificationService
    {
        private readonly IBus _bus;

        public EasyNetQNotificationService(IBus bus)
        {
            _bus = bus;
        }

        public async Task<ExecutionResult> AddedEducationDocumentTypeAsync(Guid applicantId, Guid documentTypeId)
        {
            bool result = await SendingHandler(MapTo<AddedEducationDocumentTypeNotification>(applicantId, documentTypeId));

            return GiveResult(result, "An error occurred when sending a notification about the addition of an education document type.");
        }

        public async Task<ExecutionResult> ChangeEducationDocumentTypeAsync(Guid applicantId, Guid lastDocumentTypeId, Guid newDocumentTypeId)
        {
            var notification = MapTo<UpdatedEducationDocumentTypeNotification>(applicantId, newDocumentTypeId);
            notification.LastEducationDocumentTypeId = lastDocumentTypeId;

            bool result = await SendingHandler(notification);

            return GiveResult(result, "An error occurred when sending a notification about the deletion of an education document type.");
        }

        public async Task<ExecutionResult> DeletedEducationDocumentTypeAsync(Guid applicantId, Guid documentTypeId)
        {
            bool result = await SendingHandler(MapTo<DeletedEducationDocumentTypeNotification>(applicantId, documentTypeId));

            return GiveResult(result, "An error occurred when sending a notification about the deletion of an education document type.");
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

        private T MapTo<T>(Guid applicantId, Guid documentTypeId) where T : BaseEducationDocumentType, new()
        {
            return new T()
            {
                ApplicantId = applicantId,
                DocumentTypeId = documentTypeId,
            };
        }
    }
}
