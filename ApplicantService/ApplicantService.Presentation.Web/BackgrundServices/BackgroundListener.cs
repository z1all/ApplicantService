using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Domain;
using Common.ServiceBusDTOs.FromUserService;
using EasyNetQ.AutoSubscribe;

namespace ApplicantService.Presentation.Web
{
    public class BackgroundListener : IConsumeAsync<ApplicantCreatedNotification>, IConsumeAsync<UserUpdatedNotification>
    {
        private readonly IProfileRepository _profileRepository;

        public BackgroundListener(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task ConsumeAsync(ApplicantCreatedNotification message, CancellationToken cancellationToken = default)
        {
            UserCache user = new()
            {
                Id = message.Id,
                Email = message.Email,
                FullName = message.FullName,
            };

            Applicant applicant = new()
            {
                UserId = message.Id, 
            };

            await _profileRepository.CreateAsync(user, applicant);
        }

        public async Task ConsumeAsync(UserUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            bool applicantExist = await _profileRepository.AnyByIdAsync(message.Id);
            if (!applicantExist) return;

            UserCache user = new()
            {
                Id = message.Id,
                Email = message.Email,
                FullName = message.FullName,
            };

            await _profileRepository.UpdateAsync(user);
        }
    }
}
