using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using Common.ServiceBusDTOs.FromUserService;
using EasyNetQ.AutoSubscribe;

namespace ApplicantService.Presentation.Web
{
    public class BackgroundListener : IConsumeAsync<ApplicantCreatedNotification>, IConsumeAsync<UserUpdatedNotification>
    {
        private readonly IApplicantRepository _applicantRepository;

        public BackgroundListener(IApplicantRepository profileRepository)
        {
            _applicantRepository = profileRepository;
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
    }
}
