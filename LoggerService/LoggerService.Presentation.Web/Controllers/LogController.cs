using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoggerService.Core.Application.DTOs;
using LoggerService.Core.Application.Interfaces;
using Common.API.Controllers;
using Common.Models.Enums;

namespace LoggerService.Presentation.Web.Controllers
{
    [Route("api/log")]
    [ApiController]
    [Authorize(Roles = Role.Admin)]
    public class LogController : BaseController
    {
        private readonly ILogService _logService;
        
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("logs")]
        public async Task<ActionResult<LogPagedDTO>> GetLogs([FromQuery] LogFilterDTO logFilter)
        {
            return await ExecutionResultHandlerAsync(async _ => await _logService.GetLogsAsync(logFilter));
        }
    }
}
