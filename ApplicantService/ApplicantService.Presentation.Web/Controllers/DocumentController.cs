using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Services;
using Common.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantService.Presentation.Web.Controllers
{
    [Route("api/document")]
    [ApiController]
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
        [Authorize]
        public async Task<ActionResult<List<DocumentInfo>>> GetDocuments()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{documentId}")]
        [Authorize]
        public async Task<ActionResult> DeleteDocument([FromRoute] Guid documentId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("passport")]
        [Authorize]
        public async Task<ActionResult<PassportInfo>> GetPassport()
        {
            throw new NotImplementedException();
        }

        [HttpPut("passport")]
        [Authorize]
        public async Task<ActionResult> EditPassport(EditAddPassportInfo passportInfo)
        {
            throw new NotImplementedException();
        }

        [HttpPost("passport")]
        [Authorize]
        public async Task<ActionResult> AddPassport(EditAddPassportInfo passportInfo)
        {
            throw new NotImplementedException();
        }

        [HttpGet("education/{documentId}")]
        [Authorize]
        public async Task<ActionResult<EducationDocumentInfo>> GetEducationDocument([FromRoute] Guid documentId)
        {
            throw new NotImplementedException();
        }

        [HttpPut("education/{documentId}")]
        [Authorize]
        public async Task<ActionResult> EditEducationDocument([FromRoute] Guid documentId, EditAddEducationDocumentInfo documentInfo)
        {
            throw new NotImplementedException();
        }

        [HttpPost("education/{documentId}")]
        [Authorize]
        public async Task<ActionResult> AddEducationDocument([FromRoute] Guid documentId, EditAddEducationDocumentInfo documentInfo)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{documentId}/scan/{scanId}")]
        [Authorize]
        public async Task<ActionResult> GetScan([FromRoute] Guid documentId, [FromRoute] Guid scanId)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{documentId}/scan/{scanId}")]
        [Authorize]
        public async Task<ActionResult> DeleteScan([FromRoute] Guid documentId, [FromRoute] Guid scanId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{documentId}/scan")]
        [Authorize]
        public async Task<ActionResult> AddScan([FromRoute] Guid documentId)
        {
            throw new NotImplementedException();
        }
    }
}