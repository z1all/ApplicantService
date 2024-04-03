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
            bool applicantExist = await _applicantRepository.AnyByIdAsync(message.Id);
            if (!applicantExist) return;

            Applicant applicant = new()
            {
                Id = message.Id,
                Email = message.Email,
                FullName = message.FullName,
            };

            await _applicantRepository.UpdateAsync(applicant);
        }
    }
}
