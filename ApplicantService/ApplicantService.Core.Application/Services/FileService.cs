using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Application.Interfaces.Services;
using ApplicantService.Core.Application.Mapper;
using ApplicantService.Core.Domain;
using Common.Models.Models;

namespace ApplicantService.Core.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IRequestService _requestService;
        private readonly IFileRepository _fileRepository;
        private readonly IDocumentRepository _documentRepository;

        public FileService(IRequestService requestService, IFileRepository fileRepository, IDocumentRepository documentRepository)
        {
            _requestService = requestService;
            _fileRepository = fileRepository;
            _documentRepository = documentRepository;
        }

        public async Task<ExecutionResult<FileDTO>> GetApplicantScanAsync(Guid documentId, Guid scanId, Guid applicantId)
        {
            ExecutionResult<DocumentFileInfo> executionResult = await GetDocumentFileInfoAsync(documentId, scanId, applicantId);
            if (!executionResult.IsSuccess)
            {
                return new() { Errors = executionResult.Errors };
            }

            DocumentFileInfo documentFileInfo = executionResult.Result!;
            FileEntity? fileEntity = await _fileRepository.GetFileAsync(documentFileInfo);
            if (fileEntity is null)
            {
                return new(keyError: "FileScanNotFound", error: $"Applicant doesn't have file of scan with id {scanId} in document with id {documentId}");
            }

            return new() { Result = fileEntity.ToFileDTO(documentFileInfo) };
        }

        public async Task<ExecutionResult> DeleteApplicantScanAsync(Guid documentId, Guid scanId, Guid applicantId, Guid? managerId = null)
        {
            ExecutionResult canEdit = await _requestService.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new() { Errors = canEdit.Errors };
            }

            ExecutionResult<DocumentFileInfo> executionResult = await GetDocumentFileInfoAsync(documentId, scanId, applicantId);
            if(!executionResult.IsSuccess)
            {
                return new() { Errors = executionResult.Errors };
            }

            DocumentFileInfo documentFileInfo = executionResult.Result!;
            await _fileRepository.DeleteAsync(documentFileInfo);

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> AddApplicantScanAsync(Guid documentId, Guid applicantId, FileDTO file, Guid? managerId = null)
        {
            ExecutionResult canEdit = await _requestService.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess)
            {
                return new() { Errors = canEdit.Errors };
            }

            bool documentExist = await _documentRepository.AnyByDocumentIdAndApplicantIdAsync(documentId, applicantId);
            if (!documentExist)
            {
                return new(keyError: "DocumentNotFound", error: $"Applicant doesn't have document with id {documentId}");
            }

            var (fileEntity, documentFileInfo) = file.ToFileEntityAndDocumentFileInfo(documentId);
            await _fileRepository.AddAsync(documentFileInfo, fileEntity);

            return new(isSuccess: true);
        }

        private async Task<ExecutionResult<DocumentFileInfo>> GetDocumentFileInfoAsync(Guid documentId, Guid scanId, Guid applicantId)
        {
            bool documentExist = await _documentRepository.AnyByDocumentIdAndApplicantIdAsync(documentId, applicantId);
            if (!documentExist)
            {
                return new(keyError: "DocumentNotFound", error: $"Applicant doesn't have document with id {documentId}");
            }

            DocumentFileInfo? documentFileInfo = await _fileRepository.GetInfoByFileIdAndDocumentIdAsync(scanId, documentId);
            if (documentFileInfo is null)
            {
                return new(keyError: "ScanNotFound", error: $"Applicant doesn't have scan with id {scanId} in document with id {documentId}");
            }

            return new() { Result = documentFileInfo };
        }
    }
}