using ApplicantService.Core.Application.Interfaces.Services;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Notifications;
using EasyNetQ.AutoSubscribe;

namespace ApplicantService.Presentation.Web
{
    public class BackgroundListener : IConsumeAsync<ApplicantCreatedNotification>, IConsumeAsync<UserUpdatedNotification>, IConsumeAsync<EducationDocumentTypeUpdatedNotification>
    {
        private readonly IApplicantProfileService _applicantProfileService;
        private readonly IDocumentService _documentService;

        public BackgroundListener(IApplicantProfileService applicantProfileService, IDocumentService documentService)
        {
            _applicantProfileService = applicantProfileService;
            _documentService = documentService;
        }

        public async Task ConsumeAsync(ApplicantCreatedNotification message, CancellationToken cancellationToken = default)
        {
            await _applicantProfileService.CreateApplicantAsync(new()
            {
                Id = message.Id,
                FullName = message.FullName,
                Email = message.Email,
            });
        }

        public async Task ConsumeAsync(UserUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            await _applicantProfileService.UpdateApplicantAsync(new()
            {
                Id = message.Id,
                FullName = message.FullName,
                Email = message.Email,
            });
        }

        public async Task ConsumeAsync(EducationDocumentTypeUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            await _documentService.UpdateEducationDocumentType(new()
            {
                Id = message.EducationDocumentType.Id,
                Name = message.EducationDocumentType.Name,
                Deprecated = message.Deprecated,
            });
        }
    }
}
