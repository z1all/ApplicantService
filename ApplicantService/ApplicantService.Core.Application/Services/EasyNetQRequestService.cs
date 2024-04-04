using ApplicantService.Core.Application.Interfaces.Services;
using Common.Models;
using EasyNetQ;

namespace ApplicantService.Core.Application.Services
{
    public class EasyNetQRequestService : IRequestService
    {
        private readonly IBus _bus;

        public EasyNetQRequestService(IBus bus)
        {
            _bus = bus;
        }

        public Task<ExecutionResult<bool>> CheckAdmissionStatusIsCloseAsync(Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult<bool>> CheckManagerEditPermissionAsync(Guid applicantId, Guid managerId)
        {
            throw new NotImplementedException();
        }
    }
}
