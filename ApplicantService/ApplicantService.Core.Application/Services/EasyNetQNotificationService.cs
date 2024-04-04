using ApplicantService.Core.Application.Interfaces.Services;
using EasyNetQ;

namespace ApplicantService.Core.Application.Services
{
    public class EasyNetQNotificationService : INotificationService
    {
        private readonly IBus _bus;

        public EasyNetQNotificationService(IBus bus)
        {
            _bus = bus;
        }
    }
}
