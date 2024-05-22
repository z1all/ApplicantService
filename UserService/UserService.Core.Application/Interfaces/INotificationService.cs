using Common.Models.DTOs;
using Common.Models.Models;

namespace UserService.Core.Application.Interfaces
{
    public interface INotificationService
    {
        Task<ExecutionResult> CreatedApplicantAsync(UserDTO user);
        Task<ExecutionResult> CreatedManagerAsync(UserDTO user, string password); // Для сообщения в сервис уведомлений
        Task<ExecutionResult> UpdatedUserAsync(UserDTO user);
    }
}
