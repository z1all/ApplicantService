using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Core.Application.Mappers;
using AdmissioningService.Core.DictionaryHelpers;
using AdmissioningService.Core.Domain;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;

namespace AdmissioningService.Core.Application.Services
{
    public class AdmissionService : IAdmissionService
    {
        private readonly IAdmissionCompanyRepository _companyRepository;
        private readonly IApplicantCacheRepository _applicantCacheRepository;
        private readonly IApplicantAdmissionStateMachin _applicantAdmissionStateMachin;
        private readonly IRequestService _requestService;

        private readonly DictionaryHelper _dictionaryHelper;

        public AdmissionService(
            IAdmissionCompanyRepository companyRepository, IApplicantCacheRepository applicantCacheRepository,
            IApplicantAdmissionStateMachin applicantAdmissionStateMachin,
            IRequestService requestService, DictionaryHelper dictionaryHelper)
        {
            _companyRepository = companyRepository;
            _applicantCacheRepository = applicantCacheRepository;
            _applicantAdmissionStateMachin = applicantAdmissionStateMachin;
            _requestService = requestService;
            _dictionaryHelper = dictionaryHelper;
        }

        public async Task<ExecutionResult<List<AdmissionCompanyDTO>>> GetAdmissionCompaniesAsync(Guid applicantId)
        {
            List<AdmissionCompany> admissionCompanies = await _companyRepository.GetAllWithApplicantAdmissionAsync(applicantId);

            return new()
            {
                Result = admissionCompanies
                            .Select(admissionCompany => admissionCompany.ToAdmissionCompanyDTO())
                            .ToList()
            };
        }

        public async Task<ExecutionResult> CreateAdmissionAsync(Guid applicantId)
        {
            AdmissionCompany? admissionCompany = await _companyRepository.GetCurrentAsync();
            if (admissionCompany is null)
            {
                return new(keyError: "NotExistCurrentAdmission", error: "There is no current admissions company at the moment. Try again later!");
            }

            ApplicantAdmission? applicantAdmission = await _applicantAdmissionStateMachin.GetByAdmissionCompanyId(admissionCompany.Id);
            if (applicantAdmission is not null)
            {
                return new(keyError: "AdmissionAlreadyExist", error: "The applicant already has an admission in the current admission company!");
            }

            ExecutionResult checkingResult = await CheckApplicantAsync(applicantId);
            if(!checkingResult.IsSuccess) return new() { Errors = checkingResult.Errors };

            await _applicantAdmissionStateMachin.AddAsync(applicantId, admissionCompany);

            return new(isSuccess: true);
        }



        public Task<ExecutionResult> AddProgramToAdmissionAsync(Guid applicantId, Guid admissionId, Guid programId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> ChangeAdmissionProgramPriorityAsync(Guid applicantId, Guid admissionId, ChangePrioritiesApplicantProgramDTO changePriorities)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> DeleteAdmissionProgramAsync(Guid applicantId, Guid admissionId, Guid programId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult<ApplicantAdmissionDTO>> GetApplicantAdmissionAsync(Guid applicantId, Guid admissionId)
        {
            throw new NotImplementedException();
        }

        private async Task<ExecutionResult> CheckApplicantAsync(Guid applicantId)
        {
            ApplicantCache? applicant = await _applicantCacheRepository.GetByIdAsync(applicantId);
            if (applicant is null)
            {
                ExecutionResult<GetApplicantResponse> result = await _requestService.GetApplicantAsync(applicantId);
                if (!result.IsSuccess) return new() { Errors = result.Errors };
                GetApplicantResponse applicantResponse = result.Result!;

                applicant = new()
                {
                    Id = applicantResponse.Id,
                    Email = applicantResponse.Email,
                    FullName = applicantResponse.FullName,
                    AddedDocumentTypes = await _dictionaryHelper.ToDocumentTypeFromDbAsync(applicantResponse.AddedDocumentTypesId)
                };

                await _applicantCacheRepository.AddAsync(applicant);
            }

            return new(isSuccess: true);
        }
    }
}
