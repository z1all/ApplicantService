using AdmissioningService.Core.Application.Helpers;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Core.Domain;
using Common.Models.DTOs;
using Common.Models.Models;

namespace AdmissioningService.Core.Application.Services
{
    public class AdmissionBackgroundService : IAdmissionBackgroundService
    {
        private readonly IUserCacheRepository _userCacheRepository;
        private readonly IApplicantCacheRepository _applicantCacheRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;
        private readonly IAdmissionProgramRepository _admissionProgramRepository;
        private readonly IApplicantAdmissionStateMachin _applicantAdmissionStateMachin;

        private readonly DictionaryHelper _dictionaryHelper;

        public AdmissionBackgroundService(
            IAdmissionProgramRepository admissionProgramRepository, IApplicantCacheRepository applicantCacheRepository, 
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository, IUserCacheRepository userCacheRepository,
            IApplicantAdmissionStateMachin applicantAdmissionStateMachin, DictionaryHelper dictionaryHelper)
        {
            _userCacheRepository = userCacheRepository;
            _applicantCacheRepository = applicantCacheRepository;
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
            _admissionProgramRepository = admissionProgramRepository;
            _applicantAdmissionStateMachin = applicantAdmissionStateMachin;

            _dictionaryHelper = dictionaryHelper;
        }

        public async Task UpdateUserAsync(UserDTO user)
        {
            // Один и тот же пользователь (напр. админ) может быть менеджером
            // и абитуриентом, поэтому пытаемся обновлять то и то
            UserCache? manager = await _userCacheRepository.GetByIdAsync(user.Id);
            if (manager is not null)
            {
                manager.FullName = user.FullName;
                manager.Email = user.Email;

                await _userCacheRepository.UpdateAsync(manager);
            }

            ApplicantCache? applicant = await _applicantCacheRepository.GetByIdAsync(user.Id);
            if (applicant is not null)
            {
                applicant.FullName = user.FullName;
                applicant.Email = user.Email;

                await _applicantCacheRepository.UpdateAsync(applicant);

                // В User Service абитуриент обновился, поэтому нужно обновить статус поступления
                await _applicantAdmissionStateMachin.ApplicantInfoUpdatedAsync(user.Id);
            }
        }

        public async Task ApplicantInfoUpdatedAsync(Guid applicantId)
        {
            await _applicantAdmissionStateMachin.ApplicantInfoUpdatedAsync(applicantId);
        }

        public async Task AddDocumentTypeAsync(Guid applicantId, Guid documentTypeId)
        {
            ApplicantCache? applicant = await _applicantCacheRepository.GetByIdWithDocumentTypeAsync(applicantId);
            if (applicant is not null)
            {
                EducationDocumentTypeCache? documentType = await _educationDocumentTypeCacheRepository.GetByIdAsync(documentTypeId);
                if (documentType is null)
                {
                    ExecutionResult<EducationDocumentTypeCache> result = await _dictionaryHelper.GetEducationDocumentTypeAsync(documentTypeId);
                    if (!result.IsSuccess) return;
                    documentType = result.Result!;
                }

                applicant.AddedDocumentTypes = applicant.AddedDocumentTypes.Append(documentType!).ToList();

                await _applicantCacheRepository.UpdateAsync(applicant);
            }
        }

        public async Task DeleteDocumentTypeAsync(Guid applicantId, Guid documentTypeId)
        {
            ApplicantCache? applicant = await _applicantCacheRepository.GetByIdWithDocumentTypeAndLevelsAsync(applicantId);
            if (applicant is null) return;

            applicant.AddedDocumentTypes = applicant.AddedDocumentTypes.Where(documentType => documentType.Id != documentTypeId).ToList();

            List<AdmissionProgram> admissionPrograms = await _admissionProgramRepository.GetAllByApplicantIdWithProgramWithLevelAsync(applicantId);
            foreach (var admissionProgram in admissionPrograms)
            {
                Guid programLevelId = admissionProgram.EducationProgram!.EducationLevel!.Id;

                if(!ExistRightLevel(applicant.AddedDocumentTypes, programLevelId))
                {
                    // Если из поступления удаляется программа, нужно изменить статус поступления
                    await _applicantAdmissionStateMachin.DeleteAdmissionProgramAsync(applicantId, admissionProgram);
                }
            }

            await _applicantCacheRepository.UpdateAsync(applicant);
        }

        private bool ExistRightLevel(IEnumerable<EducationDocumentTypeCache> addedDocumentTypes, Guid programLevelId)
        {
            foreach (var documentType in addedDocumentTypes)
            {
                if (documentType.EducationLevelId == programLevelId ||
                   ExistNextEducationLevel(documentType.NextEducationLevel, programLevelId))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ExistNextEducationLevel(IEnumerable<EducationLevelCache> educationLevels, Guid programLevelId)
        {
            foreach(var educationLevel in educationLevels)
            {
                if (educationLevel.Id == programLevelId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
