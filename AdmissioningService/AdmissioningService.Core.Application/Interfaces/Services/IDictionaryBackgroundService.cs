using Common.Models.DTOs;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface IDictionaryBackgroundService
    {
        Task AddEducationLevelAndEducationDocumentTypeAsync(List<EducationLevelDTO> educationLevels, List<EducationDocumentTypeDTO> documentTypes);
        Task UpdateEducationDocumentTypeAsync(EducationDocumentTypeDTO newDocumentType, bool Deprecated);
        Task UpdateEducationLevelAsync(EducationLevelDTO newEducationLevel, bool Deprecated);
        Task UpdateEducationProgramAsync(EducationProgramDTO newEducationProgram, bool Deprecated);
        Task UpdateFacultyAsync(FacultyDTO newFaculty, bool Deprecated);
    }
}
