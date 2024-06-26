﻿using Microsoft.Extensions.Logging;
using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Application.Interfaces.Services;
using ApplicantService.Core.Application.Mapper;
using ApplicantService.Core.Domain;
using Common.Models.DTOs.Applicant;
using Common.Models.Models;

namespace ApplicantService.Core.Application.Services
{
    public class ApplicantProfileService : IApplicantProfileService
    {
        private readonly ILogger<ApplicantProfileService> _logger;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IEducationDocumentRepository _educationDocumentRepository;
        private readonly IRequestService _requestService;
        private readonly INotificationService _notificationService;

        public ApplicantProfileService(
            ILogger<ApplicantProfileService> logger,
            IApplicantRepository profileRepository, IEducationDocumentRepository educationDocumentRepository,
            IRequestService requestService, INotificationService notificationService)
        {
            _logger = logger;
            _applicantRepository = profileRepository;
            _educationDocumentRepository = educationDocumentRepository;
            _requestService = requestService;
            _notificationService = notificationService;
        }

        public async Task<ExecutionResult<ApplicantProfile>> GetApplicantProfileAsync(Guid applicantId)
        {
            Applicant? applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if (applicant == null)
            {
                _logger.LogWarning($"Applicant with id {applicantId} not found");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ProfileNotFound", error: "Applicant not found! Try again later.");
            }

            return new(result: applicant.ToApplicantProfile());
        }

        public async Task<ExecutionResult> EditApplicantProfileAsync(EditApplicantProfile applicantProfile, Guid applicantId, Guid? managerId)
        {
            ExecutionResult canEdit = await _requestService.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new(canEdit.StatusCode, errors: canEdit.Errors);
            }

            Applicant? applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if (applicant == null)
            {
                _logger.LogWarning($"Applicant with id {applicantId} not found");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ProfileNotFound", error: "Applicant not found! Try again later.");
            }

            applicant.Birthday = applicantProfile.Birthday;
            applicant.Citizenship = applicantProfile.Citizenship;
            applicant.Gender = applicantProfile.Gender;
            applicant.PhoneNumber = applicantProfile.PhoneNumber;

            await _applicantRepository.UpdateAsync(applicant);

            var updateResult = await _notificationService.UpdatedApplicantInfoAsync(applicantId);

            if(updateResult.IsSuccess)
            {
                _logger.LogInformation($"Applicant with id {applicantId} was updated");
            }

            return updateResult;
        }

        public async Task<ExecutionResult<ApplicantAndAddedDocumentTypesDTO>> GetApplicantAndAddedDocumentTypesAsync(Guid applicantId)
        {
            Applicant? applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if (applicant is null)
            {
                _logger.LogWarning($"Applicant with id {applicantId} not found");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ApplicantNotFound", error: $"Applicant with id {applicantId} not found!");
            }

            List<EducationDocument> educationDocuments = await _educationDocumentRepository.GetAllByApplicantIdAsync(applicantId);

            return new()
            {
                Result = new()
                {
                    Id = applicant.Id,
                    FullName = applicant.FullName,
                    Email = applicant.Email,
                    AddedDocumentTypesId = educationDocuments.Select(document => document.EducationDocumentType!.Id).ToList()
                },
            };
        }

        public async Task CreateApplicantAsync(UserDTO user)
        {
            Applicant applicant = new()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
            };

            await _applicantRepository.AddAsync(applicant);

            _logger.LogInformation($"Applicant with id {user.Id} was created");
        }

        public async Task UpdateApplicantAsync(UserDTO newUser)
        {
            Applicant? applicant = await _applicantRepository.GetByIdAsync(newUser.Id);
            if (applicant is null) return;

            applicant.FullName = newUser.FullName;
            applicant.Email = newUser.Email;

            await _applicantRepository.UpdateAsync(applicant);

            _logger.LogInformation($"Applicant with id {newUser.Id} was updated");
        }

        public async Task<ExecutionResult<ApplicantInfo>> GetApplicantInfoAsync(Guid applicantId)
        {
            Applicant? applicant = await _applicantRepository.GetByIdWithDocumentsAsync(applicantId);
            if (applicant is null)
            {
                _logger.LogWarning($"Applicant with id {applicantId} not found");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ApplicantNotFound", error: $"Applicant with id {applicantId} not found!");
            }

            return new(result: applicant.ToApplicantInfo());
        }
    }
}
