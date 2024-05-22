using ApplicantService.Core.Application.Interfaces.Services;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Base;
using Common.ServiceBus.NotificationSender;
using Common.Models.Models;
using EasyNetQ;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Notifications;

namespace ApplicantService.Core.Application.Services
{
    public class EasyNetQNotificationService : NotificationSender, INotificationService
    {
        public EasyNetQNotificationService(IBus bus) : base(bus) { }

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

        public async Task<ExecutionResult> UpdatedApplicantInfoAsync(Guid applicantId)
        {
            bool result = await SendingHandler(new ApplicantInfoUpdatedNotification() { ApplicantId = applicantId });

            return GiveResult(result, "An error occurred when sending a notification about applicant info was updated.");
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
