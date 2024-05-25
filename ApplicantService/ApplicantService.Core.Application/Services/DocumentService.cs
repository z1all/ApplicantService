using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Extensions;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Application.Interfaces.Services;
using ApplicantService.Core.Application.Mapper;
using ApplicantService.Core.Application.Mappers;
using ApplicantService.Core.Domain;
using Common.Models.DTOs.Applicant;
using Common.Models.Enums;
using Common.Models.Models;

namespace ApplicantService.Core.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentFileInfoRepository _documentFileInfoRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IPassportRepository _passportRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IEducationDocumentRepository _educationDocumentRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;
        private readonly IRequestService _requestService;
        private readonly INotificationService _notificationService;

        public DocumentService(
            IDocumentFileInfoRepository documentFileInfoRepository,
            IDocumentRepository documentRepository, IPassportRepository passportRepository,
            IFileRepository fileRepository, IEducationDocumentRepository educationDocumentRepository, 
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository,
            IRequestService requestService, INotificationService notificationService)
        {
            _documentFileInfoRepository = documentFileInfoRepository;
            _documentRepository = documentRepository;
            _passportRepository = passportRepository;
            _fileRepository = fileRepository;
            _educationDocumentRepository = educationDocumentRepository;
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
            _requestService = requestService;
            _notificationService = notificationService;
        }

        public async Task<ExecutionResult> DeleteApplicantDocumentAsync(Guid documentId, Guid applicantId, Guid? managerId)
        {
            ExecutionResult canEdit = await _requestService.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new(canEdit.StatusCode, errors: canEdit.Errors);
            }

            Document? document = await _documentRepository.GetByDocumentIdAndApplicantIdAsync(documentId, applicantId);
            if (document is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "DeleteDocumentFail", error: $"The applicant does not have a document with Id {documentId}.");
            }

            Guid educationDocumentTypeId = Guid.Empty;
            if (document.DocumentType == DocumentType.EducationDocument &&
                !(await TryGetEducationDocumentTypeIdAsync(documentId)).TryOut(out educationDocumentTypeId))
            {
                return new(StatusCodeExecutionResult.InternalServer, keyError: "UnknowError", error: "Unknow error.");
            }

            await _fileRepository.DeleteAllFromDocumentAsync(documentId);

            if(document.DocumentType == DocumentType.EducationDocument)  
            {
                await _notificationService.DeletedEducationDocumentTypeAsync(applicantId, educationDocumentTypeId);
            }

            await _documentRepository.DeleteAsync(document);

            return await _notificationService.UpdatedApplicantInfoAsync(applicantId);
        }

        public async Task<ExecutionResult<List<DocumentInfo>>> GetApplicantDocumentsAsync(Guid applicantId)
        {
            List<Document> documents = await _documentRepository.GetAllByApplicantIdAsync(applicantId);
            
            return new()
            {
                Result = documents.Select(document => new DocumentInfo() 
                { 
                    Id = document.Id,
                    Type = document.DocumentType,
                    Comments = document.Comments,
                }).ToList()
            };
        }       

        public async Task<ExecutionResult<PassportInfo>> GetApplicantPassportAsync(Guid applicantId)
        {
            Passport? passport = await _passportRepository.GetByApplicantIdAsync(applicantId);
            if(passport is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "PassportNotFound", error: "Applicant doesn't have a passport");
            }

            return new(result: passport.ToPassportInfo());
        }

        public async Task<ExecutionResult> AddApplicantPassportAsync(EditAddPassportInfo documentInfo, Guid applicantId, Guid? managerId)
        {
            ExecutionResult canEdit = await _requestService.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new(canEdit.StatusCode, errors: canEdit.Errors);
            }

            bool passportExist = await _passportRepository.AnyByApplicantIdAsync(applicantId);
            if (passportExist)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "PassportAlreadyExist", error: "Applicant already have a passport");
            }

            Passport passport = documentInfo.ToPassport(applicantId);
            await _passportRepository.AddAsync(passport);

            return await _notificationService.UpdatedApplicantInfoAsync(applicantId);
        }

        public async Task<ExecutionResult> UpdateApplicantPassportAsync(EditAddPassportInfo documentInfo, Guid applicantId, Guid? managerId)
        {
            ExecutionResult canEdit = await _requestService.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new(canEdit.StatusCode, errors: canEdit.Errors);
            }

            Passport? passport = await _passportRepository.GetByApplicantIdAsync(applicantId);
            if (passport is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "PassportNotFound", error: "Applicant doesn't have a passport");
            }

            passport.BirthPlace = documentInfo.BirthPlace;
            passport.IssuedByWhom = documentInfo.IssuedByWhom;
            passport.IssueYear = documentInfo.IssueYear;
            passport.SeriesNumber = documentInfo.SeriesNumber;

            await _passportRepository.UpdateAsync(passport);

            return await _notificationService.UpdatedApplicantInfoAsync(applicantId);
        }

        public async Task<ExecutionResult<EducationDocumentInfo>> GetApplicantEducationDocumentAsync(Guid documentId, Guid applicantId)
        {
            EducationDocument? educationDocument = await _educationDocumentRepository.GetByDocumentIdAndApplicantIdAsync(documentId, applicantId);
            if (educationDocument is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "EducationDocumentNotFound", error: $"The applicant does not have a education document with Id {documentId}");
            }

            return new() { Result = educationDocument.ToEducationDocumentInfo() };
        }

        public async Task<ExecutionResult> AddApplicantEducationDocumentAsync(Guid applicantId, AddEducationDocumentInfo documentInfo, Guid? managerId) 
        {
            ExecutionResult<EducationDocumentTypeCache> executionResult 
                = await CheckPermissionsAndEducationDocumentTypeAsync(applicantId, documentInfo, managerId);
            if (!executionResult.IsSuccess)
            {
                return new(executionResult.StatusCode, errors: executionResult.Errors);
            }

            bool documentExist = await _educationDocumentRepository.AnyByDocumentTypeIdAndApplicantIdAsync(documentInfo.EducationDocumentTypeId, applicantId);
            if (documentExist)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "EducationDocumentAlreadyExist", error: "Applicant already has the education document with this type!");
            }

            EducationDocument educationDocument = documentInfo.ToEducationDocument(applicantId, executionResult.Result!.Name);
            await _educationDocumentRepository.AddAsync(educationDocument);

            ExecutionResult notificationResult = await _notificationService.AddedEducationDocumentTypeAsync(applicantId, documentInfo.EducationDocumentTypeId);
            if (!notificationResult.IsSuccess)
            {
                return new(notificationResult.StatusCode, errors: notificationResult.Errors);
            }

            return await _notificationService.UpdatedApplicantInfoAsync(applicantId);
        }

        public async Task<ExecutionResult> UpdateApplicantEducationDocumentAsync(Guid documentId, Guid applicantId, EditEducationDocumentInfo documentInfo, Guid? managerId)
        {
            ExecutionResult canEdit = await _requestService.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new(canEdit.StatusCode, errors: canEdit.Errors);
            }

            //ExecutionResult<EducationDocumentTypeCache> executionResult 
            //    = await CheckPermissionsAndEducationDocumentTypeAsync(applicantId, documentInfo, managerId);
            //if (!executionResult.IsSuccess)
            //{
            //    return new(executionResult.StatusCode, errors: executionResult.Errors);
            //}

            EducationDocument? educationDocument = await _educationDocumentRepository.GetByIdAsync(documentId);
            if (educationDocument is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "EducationDocumentNotFound", error: $"The applicant does not have a education document with Id {documentId}");
            }

            //bool documentExist = await _educationDocumentRepository.AnyByDocumentTypeIdAndApplicantIdAsync(documentInfo.EducationDocumentTypeId, applicantId);
            //if (educationDocument.EducationDocumentTypeId != documentInfo.EducationDocumentTypeId && documentExist)
            //{
            //    return new(StatusCodeExecutionResult.BadRequest, keyError: "EducationDocumentAlreadyExist", error: "Applicant already has the education document with this type!");
            //}

            //Guid LastEducationDocumentTypeId = educationDocument.EducationDocumentTypeId;

            //educationDocument.EducationDocumentTypeId = documentInfo.EducationDocumentTypeId;
            educationDocument.Name = documentInfo.Name;
            //educationDocument.Comments = executionResult.Result!.Name;

            await _educationDocumentRepository.UpdateAsync(educationDocument);

            //ExecutionResult notificationResult 
            //    = await _notificationService.ChangeEducationDocumentTypeAsync(applicantId, LastEducationDocumentTypeId, documentInfo.EducationDocumentTypeId);
            //if (!notificationResult.IsSuccess)
            //{
            //    return new(notificationResult.StatusCode, errors: notificationResult.Errors);
            //}

            return await _notificationService.UpdatedApplicantInfoAsync(applicantId);
        }

        public async Task UpdateEducationDocumentType(UpdateEducationDocumentTypeDTO newDocumentType)
        {
            EducationDocumentTypeCache? documentType = await _educationDocumentTypeCacheRepository.GetByIdAsync(newDocumentType.Id);
            if (documentType is null) return;

            documentType.Name = newDocumentType.Name;
            documentType.Deprecated = newDocumentType.Deprecated;

            await _educationDocumentTypeCacheRepository.UpdateAsync(documentType);
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

        private async Task<ExecutionResult<EducationDocumentTypeCache>> CheckPermissionsAndEducationDocumentTypeAsync(Guid applicantId, AddEducationDocumentInfo documentInfo, Guid? managerId)
        {
            ExecutionResult canEdit = await _requestService.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new(canEdit.StatusCode, errors: canEdit.Errors);
            }

            EducationDocumentTypeCache? educationDocumentTypeExist
                = await _educationDocumentTypeCacheRepository.GetByIdAsync(documentInfo.EducationDocumentTypeId);
            if (educationDocumentTypeExist is null)
            {
                ExecutionResult<EducationDocumentTypeCache> educationDocumentTypeExternal
                    = await _requestService.GetEducationDocumentTypeAsync(documentInfo.EducationDocumentTypeId);
                if (!educationDocumentTypeExternal.IsSuccess)
                {
                    return new(educationDocumentTypeExternal.StatusCode, errors: educationDocumentTypeExternal.Errors);
                }
                await _educationDocumentTypeCacheRepository.AddAsync(educationDocumentTypeExternal.Result!);

                educationDocumentTypeExist = educationDocumentTypeExternal.Result!;
            }

            return new(result: educationDocumentTypeExist);
        }

        public async Task<ExecutionResult<List<ScanInfo>>> GetScansInfoAsync(Guid applicantId, Guid documentId)
        {
            List<DocumentFileInfo> documentFiles = await _documentFileInfoRepository.GetAllByApplicantIdAndDocumentId(applicantId, documentId);

            return new()
            {
                Result = documentFiles
                            .Select(file => file.ToScanInfo())
                            .ToList(),
            };
        }
    }
}