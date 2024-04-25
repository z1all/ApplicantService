using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.DictionaryHelpers;
using AdmissioningService.Core.Domain;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;
using Common.ServiceBusDTOs.FromUserService;
using EasyNetQ.AutoSubscribe;

namespace AdmissioningService.Presentation.Web.BackgroundServices
{
    public class ApplicantBackgroundListener :
        IConsumeAsync<UserUpdatedNotification>,
        IConsumeAsync<AddedEducationDocumentTypeNotification>,
        IConsumeAsync<DeletedEducationDocumentTypeNotification>
    {
        private readonly IUserCacheRepository _userCacheRepository;
        private readonly IApplicantCacheRepository _applicantCacheRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;

        private readonly DictionaryHelper _dictionaryHelper;

        public ApplicantBackgroundListener(
            IUserCacheRepository userCacheRepository, IApplicantCacheRepository applicantCacheRepository, 
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository, DictionaryHelper dictionaryHelper)
        {
            _userCacheRepository = userCacheRepository;
            _applicantCacheRepository = applicantCacheRepository;
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;

            _dictionaryHelper = dictionaryHelper;
        }

        public async Task ConsumeAsync(UserUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            UserCache? manager = await _userCacheRepository.GetByIdAsync(message.Id);
            if (manager is not null)
            {
                manager.FullName = message.FullName;
                manager.Email = message.Email;

                await _userCacheRepository.UpdateAsync(manager);
            }
            else
            {
                ApplicantCache? applicant = await _applicantCacheRepository.GetByIdAsync(message.Id);
                if (applicant is not null)
                {
                    applicant.FullName = message.FullName;
                    applicant.Email = message.Email;

                    await _applicantCacheRepository.UpdateAsync(applicant);
                }
            }
        }

        public async Task ConsumeAsync(AddedEducationDocumentTypeNotification message, CancellationToken cancellationToken = default)
        {
            ApplicantCache? applicant = await _applicantCacheRepository.GetByIdAsync(message.ApplicantId);
            if(applicant is not null)
            {
                EducationDocumentTypeCache? documentType = await _educationDocumentTypeCacheRepository.GetByIdAsync(message.DocumentTypeId);
                if (documentType is null)
                { 
                    ExecutionResult<EducationDocumentTypeCache> result = await _dictionaryHelper.GetEducationDocumentTypeAsync(message.DocumentTypeId);
                    if (!result.IsSuccess) return;
                    documentType = result.Result!;
                }

                applicant.AddedDocumentTypes = applicant.AddedDocumentTypes.Append(documentType!).ToList();

                await _applicantCacheRepository.UpdateAsync(applicant);
            }
        }

        public async Task ConsumeAsync(DeletedEducationDocumentTypeNotification message, CancellationToken cancellationToken = default)
        {
            ApplicantCache? applicant = await _applicantCacheRepository.GetByIdAsync(message.ApplicantId);
            if (applicant is not null)
            {
                applicant.AddedDocumentTypes = applicant.AddedDocumentTypes.Where(documentType => documentType.Id != message.DocumentTypeId).ToList();

                await _applicantCacheRepository.UpdateAsync(applicant);
            }
        }
    }
}
