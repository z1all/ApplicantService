using AdmissioningService.Core.Application.Interfaces.Services;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Notifications;
using EasyNetQ.AutoSubscribe;

namespace AdmissioningService.Presentation.Web.BackgroundServices
{
    public class AdmissionBackgroundListener :
        IConsumeAsync<UserUpdatedNotification>,
        IConsumeAsync<ApplicantInfoUpdatedNotification>,
        IConsumeAsync<AddedEducationDocumentTypeNotification>,
        IConsumeAsync<DeletedEducationDocumentTypeNotification>
    {
        private readonly IAdmissionBackgroundService _backgroundService;

        public AdmissionBackgroundListener(IAdmissionBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }

        public async Task ConsumeAsync(UserUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            await _backgroundService.UpdateUserAsync(new() 
            { 
                Id = message.Id,
                FullName = message.FullName,
                Email = message.Email,
            });
        }

        public async Task ConsumeAsync(ApplicantInfoUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            await _backgroundService.ApplicantInfoUpdatedAsync(message.ApplicantId);
        }

        public async Task ConsumeAsync(AddedEducationDocumentTypeNotification message, CancellationToken cancellationToken = default)
        {
            await _backgroundService.AddDocumentTypeAsync(message.ApplicantId, message.DocumentTypeId);
        }

        public async Task ConsumeAsync(DeletedEducationDocumentTypeNotification message, CancellationToken cancellationToken = default)
        {
            await _backgroundService.DeleteDocumentTypeAsync(message.ApplicantId, message.DocumentTypeId);
        }
    }
}
