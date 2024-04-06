using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Services;
using Common.Controllers;

namespace ApplicantService.Presentation.Web.Controllers
{
    [Route("api/document")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<List<DocumentInfo>>> GetDocuments()
        {
            return await ExecutionResultHandlerAsync(_documentService.GetApplicantDocumentsAsync);
        }

        [HttpDelete("{documentId}")]
        public async Task<ActionResult> DeleteDocument([FromRoute] Guid documentId)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                await _documentService.DeleteApplicantDocumentAsync(documentId, userId));
        }

        [HttpGet("passport")]
        public async Task<ActionResult<PassportInfo>> GetPassport()
        {
            return await ExecutionResultHandlerAsync(_documentService.GetApplicantPassportAsync);
        }

        [HttpPut("passport")]
        public async Task<ActionResult> EditPassport(EditAddPassportInfo passportInfo)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                 await _documentService.UpdateApplicantPassportAsync(passportInfo, userId));
        }

        [HttpPost("passport")]
        public async Task<ActionResult> AddPassport(EditAddPassportInfo passportInfo)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                await _documentService.AddApplicantPassportAsync(passportInfo, userId));
        }

        [HttpGet("education/{documentId}")]
        public async Task<ActionResult<EducationDocumentInfo>> GetEducationDocument([FromRoute] Guid documentId)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                await _documentService.GetApplicantEducationDocumentAsync(documentId, userId));
        }

        [HttpPut("education/{documentId}")]
        public async Task<ActionResult> EditEducationDocument([FromRoute] Guid documentId, EditAddEducationDocumentInfo documentInfo)
        {
            return await ExecutionResultHandlerAsync(async userId =>
                await _documentService.UpdateApplicantEducationDocumentAsync(documentId, userId, documentInfo));
        }

        [HttpPost("education")]
        public async Task<ActionResult> AddEducationDocument([FromRoute] Guid documentId, EditAddEducationDocumentInfo documentInfo)
        {
            return await ExecutionResultHandlerAsync(async userId =>
               await _documentService.AddApplicantEducationDocumentAsync(documentId, documentInfo));
        }

        [HttpGet("{documentId}/scan/{scanId}")]
        public async Task<ActionResult> GetScan([FromRoute] Guid documentId, [FromRoute] Guid scanId)
        {
            

            throw new NotImplementedException();
        }

        [HttpDelete("{documentId}/scan/{scanId}")]
        public async Task<ActionResult> DeleteScan([FromRoute] Guid documentId, [FromRoute] Guid scanId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{documentId}/scan")]
        public async Task<ActionResult> AddScan([FromRoute] Guid documentId, IFormFile formFile)
        {
            return await ExecutionResultHandlerAsync(async applicantId =>
            {
                FileDTO file = new()
                {
                    Name = Path.GetFileNameWithoutExtension(formFile.FileName),
                    Type = Path.GetExtension(formFile.FileName),
                    File = await GetFileAsync(formFile),
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