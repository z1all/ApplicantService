using ApplicantService.Core.Application.Interfaces.Services;
using Common.Models;
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

        public Task<ExecutionResult> AddedEducationDocumentTypeAsync(Guid applicantId, Guid documentTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> ChangeEducationDocumentTypeAsync(Guid applicantId, Guid lastDocumentTypeId, Guid newDocumentTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> DeletedEducationDocumentTypeAsync(Guid applicantId, Guid documentTypeId)
        {
            throw new NotImplementedException();
        }
    }
}
