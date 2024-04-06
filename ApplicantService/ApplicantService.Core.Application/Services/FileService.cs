using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Repositories;
using ApplicantService.Core.Application.Interfaces.Services;
using ApplicantService.Core.Domain;
using Common.Models;

namespace ApplicantService.Core.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IDocumentRepository _documentRepository;

        public FileService(IFileRepository fileRepository, IDocumentRepository documentRepository)
        {
            _fileRepository = fileRepository;
            _documentRepository = documentRepository;
        }

        public Task<ExecutionResult> GetApplicantScanAsync(Guid documentId, Guid scanId, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> DeleteApplicantScanAsync(Guid documentId, Guid scanId, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecutionResult> AddApplicantScanAsync(Guid documentId, Guid applicantId, FileDTO file)
        {
            bool documentExist = await _documentRepository.AnyByDocumentIdAndApplicantIdAsync(documentId, applicantId);
            if (!documentExist)
            {
                return new(keyError: "DocumentNotFound", error: $"Applicant doesn't have document with id {documentId}");
            }

            DocumentFileInfo documentFileInfo = new()
            {
                DocumentId = documentId,
                Name = file.Name,
                Type = file.Type,
            };

            FileEntity fileEntity = new()
            {
                File = file.File
            };

            await _fileRepository.AddAsync(documentFileInfo, fileEntity);

            return new(isSuccess: true);
        }
    }
}
