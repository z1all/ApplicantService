using Microsoft.AspNetCore.Mvc;
using EasyNetQ;
using Common.Models.Models;
using Common.Models.Enums;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;
using Common.Models.DTOs.Dictionary;

namespace DictionaryService.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController(IBus bus /*IUpdateDictionaryService _updateDictionaryService*/) : ControllerBase
    {
        // В класс нужно пропихнуть IServiceProvider
        // Класс регаем как синглтон
        // в этом классе bus.Rpc.Respond<MyRequest, MyResponse>() передаем лямбду, в которой через IServiceProvider получаем нужный сервис 

        [HttpPost]
        public async Task<ActionResult> Update(DictionaryType dictionaryType)
        {
            await bus.PubSub.PublishAsync<UpdateDictionaryNotificationRequest>(new()
            {
                DictionaryType = dictionaryType
            });

            return NoContent();
        }

        [HttpPost("all")]
        public async Task<ActionResult> UpdateAll()
        {
            await bus.PubSub.PublishAsync<UpdateAllDictionaryNotificationRequest>(new());

            return NoContent();
        }

        [HttpGet]
        public async Task<List<UpdateStatusDTO>> GetStatuses()
        {
            var response = await bus.Rpc.RequestAsync<GetUpdateStatusesRequest, ExecutionResult<GetUpdateStatusesResponse>>(new());

            return response.Result!.UpdateStatuses;
        }

        [HttpGet("Faculty")]
        public async Task<List<FacultyDTO>> GetFaculty()
        {
            var response = await bus.Rpc.RequestAsync<GetFacultiesRequest, ExecutionResult<GetFacultiesResponse>>(new());

            return response.Result!.Faculties;
        }

        [HttpGet("EducationPrograms")]
        public async Task<ProgramPagedDTO> GetEducationPrograms([FromQuery]EducationProgramFilterDTO filter)
        {
            var response = await bus.Rpc.RequestAsync<GetEducationProgramsRequest, ExecutionResult<GetEducationProgramsResponse>>(new()
            { 
                ProgramFilter = filter 
            });

            return response.Result!.ProgramPagedDTO;
        }

        [HttpGet("EducationLevel")]
        public async Task<List<EducationLevelDTO>> GetEducationLevel()
        {
            var response = await bus.Rpc.RequestAsync<GetEducationLevelsRequest, ExecutionResult<GetEducationLevelsResponse>>(new());

            return response.Result!.EducationLevels;
        }

        [HttpGet("DocumentTypes")]
        public async Task<List<EducationDocumentTypeDTO>> GetDocumentTypes()
        {
            var response = await bus.Rpc.RequestAsync<GetDocumentTypesRequest, ExecutionResult<GetDocumentTypesResponse>>(new());

            return response.Result!.DocumentTypes;
        }
    }
}
