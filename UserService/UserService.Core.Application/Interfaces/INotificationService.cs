using UserService.Core.Application.DTOs;
using Common.Models.Models;

namespace UserService.Core.Application.Interfaces
{
    public interface INotificationService
    {
        Task<ExecutionResult> CreatedApplicantAsync(User user);
        Task<ExecutionResult> CreatedManagerAsync(User user, string password); // Для сообщения в сервис уведомлений
        Task<ExecutionResult> UpdatedUserAsync(User user);
    }
}
