using AdmissioningService.Core.Application.Interfaces.Services;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;
using Common.ServiceBusDTOs.FromUserService;
using EasyNetQ.AutoSubscribe;

namespace AdmissioningService.Presentation.Web.BackgroundServices
{
    public class AdmissionBackgroundListener :
        IConsumeAsync<UserUpdatedNotification>,
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

        public async Task ConsumeAsync(AddedEducationDocumentTypeNotification message, CancellationToken cancellationToken = default)
        {
            await _backgroundService.AddDocumentType(message.ApplicantId, message.DocumentTypeId);
        }

        public async Task ConsumeAsync(DeletedEducationDocumentTypeNotification message, CancellationToken cancellationToken = default)
        {
            await _backgroundService.DeleteDocumentType(message.ApplicantId, message.DocumentTypeId);
        }
    }
}
