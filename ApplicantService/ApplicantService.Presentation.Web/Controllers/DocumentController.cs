using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Services;
using ApplicantService.Presentation.Web.DTOs;
using Common.API.Controllers;
using Common.API.Helpers;
using Common.API.Attributes;
using Common.API.DTOs;
using Common.Models.Models;
using Common.Models.DTOs.Applicant;
using Common.Models.Enums;

namespace ApplicantService.Presentation.Web.Controllers
{
    [Route("api/document")]
    [ApiController]
    [Authorize(Roles = $"{Role.Applicant}, {Role.Admin}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class DocumentController : BaseController
    {
        private readonly IDocumentService _documentService;
        private readonly IFileService _fileService;

        public DocumentController(IDocumentService documentService, IFileService fileService)
        {
            _documentService = documentService;
            _fileService = fileService;
        }

        [HttpGet("documents")]
        [ProducesResponseType(typeof(List<DocumentInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DocumentInfo>>> GetDocuments()
        {
            return await ExecutionResultHandlerAsync(_documentService.GetApplicantDocumentsAsync);
        }

        [HttpDelete("{documentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteDocument([FromRoute] Guid documentId)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                await _documentService.DeleteApplicantDocumentAsync(documentId, userId));
        }

        [HttpGet("passport")]
        [ProducesResponseType(typeof(PassportInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PassportInfo>> GetPassport()
        {
            return await ExecutionResultHandlerAsync(_documentService.GetApplicantPassportAsync);
        }

        [HttpPut("passport")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditPassport(EditAddPassportInfo passportInfo)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                 await _documentService.UpdateApplicantPassportAsync(passportInfo, userId));
        }

        [HttpPost("passport")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddPassport(EditAddPassportInfo passportInfo)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                await _documentService.AddApplicantPassportAsync(passportInfo, userId));
        }

        [HttpGet("education/{documentId}")]
        [ProducesResponseType(typeof(EducationDocumentInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EducationDocumentInfo>> GetEducationDocument([FromRoute] Guid documentId)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                await _documentService.GetApplicantEducationDocumentAsync(documentId, userId));
        }

        [HttpPut("education/{documentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EditEducationDocument([FromRoute] Guid documentId, EditEducationDocumentInfo documentInfo)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                await _documentService.UpdateApplicantEducationDocumentAsync(documentId, userId, documentInfo));
        }

        [HttpPost("education")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddEducationDocument(AddEducationDocumentInfo documentInfo)
        {
            return await ExecutionResultHandlerAsync(async userId =>
               await _documentService.AddApplicantEducationDocumentAsync(userId, documentInfo));
        }

        [HttpGet("{documentId}/scan/{scanId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetScan([FromRoute] Guid documentId, [FromRoute] Guid scanId)
        {
            if (!HttpContext.TryGetUserId(out Guid applicantId))
            {
                return ExecutionResultHandlerAsync(new ExecutionResult(StatusCodeExecutionResult.InternalServer, "UnknowError", "Unknow error"));
            }

            ExecutionResult<FileDTO> response = await _fileService.GetApplicantScanAsync(documentId, scanId, applicantId);

            if (!response.IsSuccess) return ExecutionResultHandlerAsync(response);

            FileDTO fileDTO = response.Result!;
            if(!fileDTO.Type.TryMapToContentType(out var contentType))
            {
                return ExecutionResultHandlerAsync(new ExecutionResult(StatusCodeExecutionResult.InternalServer, "DocumentTypeError", $"Unknown document type {fileDTO.Type}"));
            }
            return File(fileDTO.File, contentType!, fileDTO.Name);
        }

        [HttpDelete("{documentId}/scan/{scanId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteScan([FromRoute] Guid documentId, [FromRoute] Guid scanId)
        {
            return await ExecutionResultHandlerAsync(async applicantId =>
                await _fileService.DeleteApplicantScanAsync(documentId, scanId, applicantId));
        }

        [HttpPost("{documentId}/scan")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddScan([FromRoute] Guid documentId, FileUpload fileUpload)
        {
            return await ExecutionResultHandlerAsync(async applicantId =>
            {
                FileDTO file = new()
                {
                    Name = Path.GetFileNameWithoutExtension(fileUpload.File.FileName),
                    Type = Path.GetExtension(fileUpload.File.FileName),
                    File = await GetFileAsync(fileUpload.File),
                };

                return await _fileService.AddApplicantScanAsync(documentId, applicantId, file);
            });
        }

        private async Task<byte[]> GetFileAsync(IFormFile formFile)
        {
            byte[] file = [];
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);
                file = stream.ToArray();   
            }
            return file;
        }
    }
}