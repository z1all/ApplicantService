using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.Filters;
using AmdinPanelMVC.DTOs;
using AmdinPanelMVC.Controllers.Base;
using AmdinPanelMVC.Models;
using Common.Models.Models;
using Common.Models.DTOs.Applicant;
using Common.API.Attributes;
using Common.API.Helpers;

namespace AmdinPanelMVC.Controllers
{
    [RequiredAuthorize]
    public class DocumentsController : BaseController
    {
        private readonly IDocumentService _documentService;
        private readonly IAdmissionService _admissionService;

        public DocumentsController(IDocumentService documentService, IAdmissionService admissionService) 
        {
            _documentService = documentService;
            _admissionService = admissionService;
        }

        public async Task<IActionResult> Passport(Guid applicantId)
        {
            if (!HttpContext.TryGetUserId(out Guid managerId))
            {
                return Redirect("/InternalServer");
            }

            ExecutionResult<PassportInfo> passport = await _documentService.GetPassportAsync(applicantId);

            if (!passport.IsSuccess)
            {
                if(passport.Errors.ContainsKey("GetPassportFail")) 
                {
                    return Redirect("/InternalServer");
                }

                return Redirect("/NotFound");
            }

            ExecutionResult editPermission = await _admissionService.CheckPermissionsAsync(applicantId, managerId);

            return View(new PassportViewModel()
            {
                ApplicantId = applicantId,
                Passport = passport.Result!,
                CanEdit = editPermission.IsSuccess,
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

        public async Task<IActionResult> EducationDocument(Guid applicantId, Guid documentId)
        {
            if (!HttpContext.TryGetUserId(out Guid managerId))
            {
                return Redirect("/InternalServer");
            }

            ExecutionResult<EducationDocumentInfo> document = await _documentService.GetEducationDocumentAsync(applicantId, documentId);

            if (!document.IsSuccess)
            {
                if (document.Errors.ContainsKey("GetEducationDocumentFail"))
                {
                    return Redirect("/InternalServer");
                }

                return Redirect("/NotFound");
            }

            ExecutionResult editPermission = await _admissionService.CheckPermissionsAsync(applicantId, managerId);

            return View(new EducationDocumentViewModel()
            {
                ApplicantId = applicantId,
                EducationDocument = document.Result!,
                CanEdit = editPermission.IsSuccess,
            });
        }

        [ValidateModelState]
        public async Task<IActionResult> ChangeEducationDocument([FromBody] ChangeEducationDocumentDTO changeEducationDocument)
        {
            return await RequestHandlerAsync((managerId)
                => _documentService.ChangeEducationDocumentAsync(changeEducationDocument, managerId));
        }

        public async Task<IActionResult> Scans(Guid applicantId, Guid documentId, bool canEdit)
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

            return PartialView("_Scans", new ScansViewModel() { Scans = scans.Result!, CanEdit = canEdit });
        }

        public async Task<IActionResult> LoadScans(Guid applicantId, Guid documentId, Guid scanId)
        {
            ExecutionResult<FileDTO> response = await _documentService.GetScanAsync(applicantId, documentId, scanId);

            if (!response.IsSuccess)
            {
                if (response.Errors.ContainsKey("GetScanFail"))
                {
                    return Redirect("/InternalServer");
                }
                return BadRequest("/NotFound");
            }

            FileDTO fileDTO = response.Result!;
            if (!fileDTO.Type.TryMapToContentType(out var contentType))
            {
                return Redirect("/InternalServer");
            }
            return File(fileDTO.File, contentType!, fileDTO.Name);
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
