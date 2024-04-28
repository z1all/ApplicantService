using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.DictionaryHelpers;
using AdmissioningService.Core.Domain;
using Common.Models.Models;

namespace AdmissioningService.Core.Application.Services
{
    public class AdmissionBackgroundService : IAdmissionBackgroundService
    {
        private readonly IUserCacheRepository _userCacheRepository;
        private readonly IApplicantCacheRepository _applicantCacheRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;
        private readonly IAdmissionProgramRepository _admissionProgramRepository;

        private readonly DictionaryHelper _dictionaryHelper;

        public AdmissionBackgroundService(
            IUserCacheRepository userCacheRepository, 
            IApplicantCacheRepository applicantCacheRepository, 
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository,
            IAdmissionProgramRepository admissionProgramRepository,
            DictionaryHelper dictionaryHelper)
        {
            _userCacheRepository = userCacheRepository;
            _applicantCacheRepository = applicantCacheRepository;
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
            _admissionProgramRepository = admissionProgramRepository;

            _dictionaryHelper = dictionaryHelper;
        }

        public async Task UpdateUserAsync(UserDTO user)
        {
            UserCache? manager = await _userCacheRepository.GetByIdAsync(user.Id);
            if (manager is not null)
            {
                manager.FullName = user.FullName;
                manager.Email = user.Email;

                await _userCacheRepository.UpdateAsync(manager);
            }
            else
            {
                ApplicantCache? applicant = await _applicantCacheRepository.GetByIdAsync(user.Id);
                if (applicant is not null)
                {
                    applicant.FullName = user.FullName;
                    applicant.Email = user.Email;

                    await _applicantCacheRepository.UpdateAsync(applicant);
                }
            }
        }

        public async Task AddDocumentType(Guid applicantId, Guid documentTypeId)
        {
            ApplicantCache? applicant = await _applicantCacheRepository.GetByIdAsync(applicantId);
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

        public async Task DeleteDocumentType(Guid applicantId, Guid documentTypeId)
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
                    await _admissionProgramRepository.DeleteAsync(admissionProgram);
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
