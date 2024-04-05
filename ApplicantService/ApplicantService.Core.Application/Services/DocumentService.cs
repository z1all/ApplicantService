﻿using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Application.Interfaces.Services;
using ApplicantService.Core.Application.Mapper;
using ApplicantService.Core.Application.Mappers;
using ApplicantService.Core.Domain;
using ApplicantService.Core.Domain.Enums;
using Common.Extensions;
using Common.Models;

namespace ApplicantService.Core.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IRequestService _requestService;
        private readonly INotificationService _notificationService;
        private readonly IDocumentRepository _documentRepository;
        private readonly IPassportRepository _passportRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IEducationDocumentRepository _educationDocumentRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;

        public DocumentService(
            IRequestService requestService, INotificationService notificationService,
            IDocumentRepository documentRepository, IPassportRepository passportRepository,
            IFileRepository fileRepository, IEducationDocumentRepository educationDocumentRepository, 
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository)
        {
            _requestService = requestService;
            _notificationService = notificationService;
            _documentRepository = documentRepository;
            _passportRepository = passportRepository;
            _fileRepository = fileRepository;
            _educationDocumentRepository = educationDocumentRepository;
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
        }

        public async Task<ExecutionResult> DeleteApplicantDocumentAsync(Guid documentId, Guid applicantId, Guid? managerId)
        {
            ExecutionResult canEdit = await CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new() { Errors = canEdit.Errors };
            }

            Document? document = await _documentRepository.GetByDocumentIdAndApplicantIdAsync(documentId, applicantId);
            if (document is null)
            {
                return new(keyError: "DeleteDocumentFail", error: $"The applicant does not have a document with Id {documentId}.");
            }

            Guid educationDocumentTypeId = Guid.Empty;
            if (document.DocumentType == DocumentType.EducationDocument &&
                (await TryGetEducationDocumentTypeIdAsync(documentId)).TryOut(out educationDocumentTypeId))
            {
                return new(keyError: "UnknowError", error: "Unknow error. Try again.");
            }

            ExecutionResult deletingResult = await _fileRepository.DeleteAllFromDocumentAsync(documentId);
            if(!deletingResult.IsSuccess) 
            {
                return new() { Errors = deletingResult.Errors };
            }

            if(document.DocumentType == DocumentType.EducationDocument)  
            {
                await _notificationService.DeletedEducationDocumentTypeAsync(applicantId, educationDocumentTypeId);
            }

            await _documentRepository.DeleteAsync(document);

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult<List<DocumentInfo>>> GetApplicantDocumentsAsync(Guid applicantId)
        {
            List<Document> documents = await _documentRepository.GetAllByApplicantIdAsync(applicantId);
            
            return new()
            {
                Result = documents.Select(document => new DocumentInfo() 
                { 
                    Id = document.Id,
                    Type = DocumentType.Passport,
                }).ToList()
            };
        }       

        public async Task<ExecutionResult<PassportInfo>> GetApplicantPassportAsync(Guid applicantId)
        {
            Passport? passport = await _passportRepository.GetByApplicantIdAsync(applicantId);
            if(passport is null)
            {
                return new(keyError: "GetPassportFail", error: "Applicant doesn't have a passport");
            }

            return new() { Result = passport.ToPassportInfo() };
        }

        public async Task<ExecutionResult> AddApplicantPassportAsync(EditAddPassportInfo documentInfo, Guid applicantId, Guid? managerId)
        {
            ExecutionResult canEdit = await CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new() { Errors = canEdit.Errors };
            }

            bool passportExist = await _passportRepository.AnyByApplicantIdAsync(applicantId);
            if (passportExist)
            {
                return new(keyError: "AddPassportFail", error: "Applicant already have a passport");
            }

            Passport passport = documentInfo.ToPassport(applicantId);
            await _passportRepository.AddAsync(passport);

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> UpdateApplicantPassportAsync(EditAddPassportInfo documentInfo, Guid applicantId, Guid? managerId)
        {
            ExecutionResult canEdit = await CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new() { Errors = canEdit.Errors };
            }

            Passport? passport = await _passportRepository.GetByApplicantIdAsync(applicantId);
            if (passport is null)
            {
                return new(keyError: "AddPassportFail", error: "Applicant doesn't have a passport");
            }

            passport.BirthPlace = documentInfo.BirthPlace;
            passport.IssuedByWhom = documentInfo.IssuedByWhom;
            passport.IssueYear = documentInfo.IssueYear;
            passport.SeriesNumber = documentInfo.SeriesNumber;

            await _passportRepository.UpdateAsync(passport);

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult<EducationDocumentInfo>> GetApplicantEducationDocumentAsync(Guid documentId, Guid applicantId)
        {
            EducationDocument? educationDocument = await _educationDocumentRepository.GetByDocumentIdAndApplicantIdAsync(documentId, applicantId);
            if (educationDocument is null)
            {
                return new(keyError: "GetEducationDocumentFail", error: $"The applicant does not have a education document with Id {documentId}");
            }

            return new() { Result = educationDocument.ToEducationDocumentInfo() };
        }

        public async Task<ExecutionResult> AddApplicantEducationDocumentAsync(Guid applicantId, EditAddEducationDocumentInfo documentInfo, Guid? managerId)
        {
            ExecutionResult executionResult = await CheckPermissionsAndEducationDocumentTypeAsync(applicantId, documentInfo, managerId);
            if (!executionResult.IsSuccess)
            {
                return new() { Errors = executionResult.Errors };
            }

            EducationDocument educationDocument = documentInfo.ToEducationDocument(applicantId);
            await _educationDocumentRepository.AddAsync(educationDocument);

            ExecutionResult notificationResult = await _notificationService.AddedEducationDocumentTypeAsync(applicantId, documentInfo.EducationDocumentTypeId);
            if (!notificationResult.IsSuccess)
            {
                return new() { Errors = notificationResult.Errors };
            }

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> UpdateApplicantEducationDocumentAsync(Guid documentId, Guid applicantId, EditAddEducationDocumentInfo documentInfo, Guid? managerId)
        {
            ExecutionResult executionResult = await CheckPermissionsAndEducationDocumentTypeAsync(applicantId, documentInfo, managerId);
            if (!executionResult.IsSuccess)
            {
                return new() { Errors = executionResult.Errors };
            }

            EducationDocument? educationDocument = await _educationDocumentRepository.GetByIdAsync(documentId);
            if (educationDocument is null)
            {
                return new(keyError: "UpdateEducationDocumentFail", error: $"The applicant does not have a education document with Id {documentId}");
            }

            Guid LastEducationDocumentTypeId = educationDocument.EducationDocumentTypeId;

            educationDocument.EducationDocumentTypeId = documentInfo.EducationDocumentTypeId;
            educationDocument.Name = documentInfo.Name;

            await _educationDocumentRepository.UpdateAsync(educationDocument);

            ExecutionResult notificationResult 
                = await _notificationService.ChangeEducationDocumentTypeAsync(applicantId, LastEducationDocumentTypeId, documentInfo.EducationDocumentTypeId);
            if (!notificationResult.IsSuccess)
            {
                return new() { Errors = notificationResult.Errors };
            }

            return new(isSuccess: true);
        }

        private async Task<ValueTuple<bool, Guid>> TryGetEducationDocumentTypeIdAsync(Guid documentId)
        {
            EducationDocument? educationDocument = await _educationDocumentRepository.GetByIdAsync(documentId);
            if (educationDocument is null)
            {
                return new(false, Guid.Empty);
            }
            
            return new(true, educationDocument.EducationDocumentTypeId);
        }

        private async Task<ExecutionResult> CheckPermissionsAndEducationDocumentTypeAsync(Guid applicantId, EditAddEducationDocumentInfo documentInfo, Guid? managerId)
        {
            ExecutionResult canEdit = await CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new() { Errors = canEdit.Errors };
            }

            bool educationDocumentTypeExist = await _educationDocumentTypeCacheRepository.AnyByIdAsync(documentInfo.EducationDocumentTypeId);
            if (!educationDocumentTypeExist)
            {
                ExecutionResult<EducationDocumentTypeCache> educationDocumentType
                    = await _requestService.GetEducationDocumentTypeAsync(documentInfo.EducationDocumentTypeId);
                if (!educationDocumentType.IsSuccess)
                {
                    return new() { Errors = educationDocumentType.Errors };
                }
                await _educationDocumentTypeCacheRepository.AddAsync(educationDocumentType.Result!);
            }

            return new(isSuccess: true);
        }

        private async Task<ExecutionResult> CheckPermissionsAsync(Guid applicantId, Guid? managerId)
        {
            if(managerId is null)
            {
                return await _requestService.CheckAdmissionStatusIsCloseAsync(applicantId);
            }
            return await _requestService.CheckManagerEditPermissionAsync(applicantId, (Guid)managerId);
        }
    }
}