using DictionaryService.Core.Domain;
using Common.Models;

namespace DictionaryService.Core.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<ExecutionResult> ChangedFacultiesAsync(Faculty faculty);
        Task<ExecutionResult> ChangedEducationProgramAsync(EducationProgram program);
        Task<ExecutionResult> ChangedEducationLevelAsync(EducationLevel educationLevel);
        Task<ExecutionResult> ChangedEducationDocumentTypeAsync(EducationDocumentType documentType);
    }
}
