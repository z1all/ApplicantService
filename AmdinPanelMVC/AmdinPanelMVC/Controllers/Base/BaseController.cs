using Microsoft.AspNetCore.Mvc;
using Common.API.DTOs;
using Common.API.Helpers;
using Common.Models.Models;

namespace AmdinPanelMVC.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        protected async Task<IActionResult> RequestHandlerAsync(
            Func<Guid, Task<ExecutionResult>> requestAsync, 
            Func<IActionResult>? OkResult = null)
        {
            if (!ModelState.IsValid || !HttpContext.TryGetUserId(out Guid managerId))
            {
                return BadRequest();
            }

            ExecutionResult result = await requestAsync(managerId);
            if (!result.IsSuccess)
            {
                ErrorResponse error = new()
                {
                    Status = 400,
                    Errors = result.Errors,
                };

                return BadRequest(error);
            }

            return OkResult is null ? Ok() : OkResult();
        }
    }
}
