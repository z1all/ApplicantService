using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.Filters;
using Common.Models.Models;
using Common.Models.DTOs.Applicant;
using AmdinPanelMVC.DTOs;
using AmdinPanelMVC.Controllers.Base;
using AmdinPanelMVC.Models;
using Common.API.Attributes;

namespace AmdinPanelMVC.Controllers
{
    [RequiredAuthorize]
    public class DocumentsController : BaseController
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService) 
        {
            _documentService = documentService;
        }

        public async Task<IActionResult> Passport(Guid applicantId)
        {
            ExecutionResult<PassportInfo> passport = await _documentService.GetPassportAsync(applicantId);

            if (!passport.IsSuccess)
            {
                if(passport.Errors.ContainsKey("GetPassportFail")) 
                {
                    return Redirect("/InternalServer");
                }

                return Redirect("/NotFound");
            }

            return View(new PassportViewModel()
            {
                ApplicantId = applicantId,
                Passport = passport.Result!
            });
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> ChangePassport([FromBody] ChangePassportDTO request)
        {
            return await RequestHandlerAsync((managerId)
                => _documentService.ChangePassportAsync(request, managerId));
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> AddFile([FromForm] AddFileDTO addFile, [FromQuery] Guid documentId, [FromQuery] Guid applicantId)
        {
            FileDTO file = new()
            {
                Name = Path.GetFileNameWithoutExtension(addFile.File.FileName),
                Type = Path.GetExtension(addFile.File.FileName),
                File = await GetFileAsync(addFile.File),
            };

            return await RequestHandlerAsync((managerId)
                => _documentService.AddDocumentScansAsync(applicantId, documentId, file, managerId));
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> DeleteFile([FromQuery] Guid documentId, [FromQuery] Guid applicantId, [FromQuery] Guid scanId)
        {
            return await RequestHandlerAsync((managerId)
                => _documentService.DeleteDocumentScansAsync(applicantId, documentId, scanId, managerId));
        }

        public IActionResult EducationDocument(Guid applicantId, Guid documentId)
        {
            return View();
        }

        public async Task<IActionResult> Scans(Guid applicantId, Guid documentId)
        {
            ExecutionResult<List<ScanInfo>> scans = await _documentService.GetDocumentScansAsync(applicantId, documentId);

            if (!scans.IsSuccess)
            {
                if (scans.Errors.ContainsKey("GetDocumentScansFail"))
                {
                    return Redirect("/InternalServer");
                }

                return Redirect("/NotFound");
            }

            return PartialView("_Scans", scans.Result);
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
