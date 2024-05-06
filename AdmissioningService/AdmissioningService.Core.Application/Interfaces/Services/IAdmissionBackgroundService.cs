using AdmissioningService.Core.Application.DTOs;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface IAdmissionBackgroundService
    {
        Task UpdateUserAsync(UserDTO user);
        Task ApplicantInfoUpdatedAsync(Guid applicantId);
        Task AddDocumentTypeAsync(Guid applicantId, Guid documentTypeId);
        Task DeleteDocumentTypeAsync(Guid applicantId, Guid documentTypeId);
    }
}
