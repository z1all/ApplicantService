using AdmissioningService.Core.Application.DTOs;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface IAdmissionBackgroundService
    {
        Task UpdateUserAsync(UserDTO user);
        Task AddDocumentType(Guid applicantId, Guid documentTypeId);
        Task DeleteDocumentType(Guid applicantId, Guid documentTypeId);
    }
}
