using Microsoft.AspNetCore.Mvc;
using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Application.Interfaces.Services;

namespace AdmissioningService.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestManagerController(IManagerService _managerService, IAdmissionService _admissionService) : ControllerBase
    {
        [HttpGet("applicant_admissions")]
        public async Task<ActionResult<ApplicantAdmissionPagedDTO>> GetApplicantAdmissions([FromQuery] ApplicantAdmissionFilterDTO admissionFilter)
        {
            Guid mainManagerId = Guid.Parse("3a48d70e-0c89-4d9e-8512-17954cca4853");
            Guid ordinaryManagerId = Guid.Parse("c10a2e03-44d8-45ab-afed-2b6fb70e3ae7");

            return Ok(await _admissionService.GetApplicantAdmissionsAsync(admissionFilter, ordinaryManagerId));
        }

        [HttpPost("take_applicant_admissions")]
        public async Task<ActionResult> TakeApplicantAdmission([FromBody] Guid admissionId)
        {
            Guid mainManagerId = Guid.Parse("3a48d70e-0c89-4d9e-8512-17954cca4853");
            Guid ordinaryManagerId = Guid.Parse("c10a2e03-44d8-45ab-afed-2b6fb70e3ae7");

            return Ok(await _managerService.TakeApplicantAdmissionAsync(admissionId, mainManagerId));
        }

        [HttpPost("refuse_applicant_admissions")]
        public async Task<ActionResult> RefuseFromApplicantAdmission([FromBody] Guid admissionId)
        {
            Guid mainManagerId = Guid.Parse("3a48d70e-0c89-4d9e-8512-17954cca4853");
            Guid ordinaryManagerId = Guid.Parse("c10a2e03-44d8-45ab-afed-2b6fb70e3ae7");

            return Ok(await _managerService.RefuseFromApplicantAdmissionAsync(admissionId, mainManagerId));
        }

        [HttpGet("managers")]
        public async Task<ActionResult> AppointFromApplicantAdmission()
        {
            return Ok(await _managerService.GetManagersAsync());
        }
    }
}
/*
    
    Переделать отправку уведомлений об изменении абитуриента

    Возможность изменить статус поступления (Закрыть/открыть)
    
    +++ Возможность посмотреть список менеджеров, главных менеджеров
    +++ Возможность назначить менеджера на поступление, если оно свободно (назначение абитуриента менеджеру)
    +++ Взятие/Отказ от поступления абитуриента
    +++ Просмотреть заявки абитуриентов с пагинацией и фильтрациями и сортировками
 */