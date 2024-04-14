using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryService.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController(IUpdateDictionaryService _updateDictionaryService) : ControllerBase
    {
        // В класс нужно пропихнуть IServiceProvider
        // Класс регаем как синглтон
        // в этом классе bus.Rpc.Respond<MyRequest, MyResponse>() передаем лямбду, в которой через IServiceProvider получаем нужный сервис 

        [HttpPost]
        public async Task<ActionResult> Update(DictionaryType dictionaryType)
        {
            return Ok(await _updateDictionaryService.UpdateDictionaryAsync(dictionaryType));
        }

        [HttpPost("all")]
        public async Task<ActionResult> UpdateAll()
        {
            return Ok(await _updateDictionaryService.UpdateAllDictionaryAsync());
        }

        [HttpGet]
        public async Task<List<UpdateStatusDTO>> GetStatuses()
        {
            return (await _updateDictionaryService.GetUpdateStatusesAsync()).Result!;
        }
    }
}
