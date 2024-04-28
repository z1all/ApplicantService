﻿using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Application.Interfaces.Services;
using ApplicantService.Core.Domain;
using Common.Models.Models;

namespace ApplicantService.Core.Application.Services
{
    public class ApplicantProfileService : IApplicantProfileService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IEducationDocumentRepository _educationDocumentRepository;

        public ApplicantProfileService(
            IApplicantRepository profileRepository, IEducationDocumentRepository educationDocumentRepository)
        {
            _applicantRepository = profileRepository;
            _educationDocumentRepository = educationDocumentRepository;
        }

        public async Task<ExecutionResult<ApplicantProfile>> GetApplicantProfileAsync(Guid applicantId)
        {
            Applicant? applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if(applicant == null)
            {
                return new(keyError: "GetProfileFail", error: "Applicant not found! Try again later.");
            }

            return new()
            {
                Result = new ApplicantProfile()
                {
                    Email = applicant.Email,
                    FullName = applicant.FullName,
                    Birthday = applicant.Birthday,
                    Citizenship = applicant.Citizenship,
                    Gender = applicant.Gender,
                    PhoneNumber = applicant.PhoneNumber,
                },
            }; 
        }

        public async Task<ExecutionResult> EditApplicantProfileAsync(EditApplicantProfile applicantProfile, Guid applicantId)
        {
            Applicant? applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if (applicant == null)
            {
                return new(keyError: "GetProfileFail", error: "Applicant not found! Try again later.");
            }

            applicant.Birthday = applicantProfile.Birthday;
            applicant.Citizenship = applicantProfile.Citizenship;
            applicant.Gender = applicantProfile.Gender;
            applicant.PhoneNumber = applicantProfile.PhoneNumber;

            await _applicantRepository.UpdateAsync(applicant);

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult<ApplicantAndAddedDocumentTypesDTO>> GetApplicantAndAddedDocumentTypesAsync(Guid applicantId)
        {
            Applicant? applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if (applicant is null)
            {
                return new(keyError: "ApplicantNotFound", error: $"Applicant with id {applicantId} not found!");
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
    }
}
