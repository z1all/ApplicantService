using Common.Models;
using UserService.Core.Application.DTOs;

namespace UserService.Core.Application.Interfaces
{
    public interface INotificationService
    {
        Task<ExecutionResult> CreatedApplicantAsync(User user);
        Task<ExecutionResult> CreatedManagerAsync(User user); // Для сообщения в сервис уведомлений
        Task<ExecutionResult> UpdatedUserAsync(User user);
    }
}
