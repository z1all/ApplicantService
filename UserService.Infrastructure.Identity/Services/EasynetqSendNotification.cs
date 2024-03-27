using EasyNetQ;
using UserService.Core.Application.Interfaces;

namespace UserService.Infrastructure.Identity.Services
{
    internal class EasynetqSendNotification : ISendNotification
    {
        private readonly IBus _bus;

        public EasynetqSendNotification(IBus bus)
        {
            _bus = bus;
        }

        public Task<bool> CreatedApplicant()
        { 
            throw new NotImplementedException();
        }

        public Task<bool> CreatedManager()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatedUser()
        {
            throw new NotImplementedException();
        }
    }
}
