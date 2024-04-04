using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using Common.ServiceBusDTOs.FromDictionaryService;
using Common.ServiceBusDTOs.FromUserService;
using EasyNetQ.AutoSubscribe;

namespace ApplicantService.Presentation.Web
{
    public class BackgroundListener : IConsumeAsync<ApplicantCreatedNotification>, IConsumeAsync<UserUpdatedNotification>, IConsumeAsync<EducationDocumentTypeUpdatedNotification>
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;

        public BackgroundListener(IApplicantRepository profileRepository, IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository)
        {
            _applicantRepository = profileRepository;
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
        }

        public async Task ConsumeAsync(ApplicantCreatedNotification message, CancellationToken cancellationToken = default)
        {
            Applicant applicant = new()
            {
                Id = message.Id,
                Email = message.Email,
                FullName = message.FullName,
            };

            await _applicantRepository.AddAsync(applicant);
        }

        public async Task ConsumeAsync(UserUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            Applicant? applicant = await _applicantRepository.GetByIdAsync(message.Id);
            if (applicant is null) return;

            applicant.FullName = message.FullName;
            applicant.Email = message.Email;

            await _applicantRepository.UpdateAsync(applicant);
        }

        public async Task ConsumeAsync(EducationDocumentTypeUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            EducationDocumentTypeCache? documentType  = await _educationDocumentTypeCacheRepository.GetByIdAsync(message.Id);
            if (documentType is null) return;

            documentType.Name = message.Name;
            documentType.Deprecated = message.Deprecated;

            await _educationDocumentTypeCacheRepository.UpdateAsync(documentType);
        }
    }
}
