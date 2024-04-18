using Microsoft.AspNetCore.Mvc;
using Common.API.Attributes;
using Common.API.DTOs;
using Common.API.Helpers;
using Common.Models;

namespace Common.API.Controllers
{
    [ApiController]
    [ValidateModelState]
    public abstract class BaseController : ControllerBase
    {
        protected BadRequestObjectResult BadRequest(ExecutionResult executionResult, string? otherMassage = null)
        {
            return BadRequest(new ErrorResponse()
            {
                Title = otherMassage ?? "One or more errors occurred.",
                Status = 400,
                Errors = executionResult.Errors,
            });
        }

        protected async Task<ActionResult<TResult>> ExecutionResultHandlerAsync<TResult>(Func<Guid, Task<ExecutionResult<TResult>>> operation)
        {
            if (!HttpContext.TryGetUserId(out Guid userId))
            {
                return BadRequest(new ExecutionResult("UnknowError", "Unknow error"));
            }

            ExecutionResult<TResult> response = await operation(userId);

            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response.Result!);
        }

        protected async Task<ActionResult<TResult>> ExecutionResultHandlerAsync<TResult>(Func<Task<ExecutionResult<TResult>>> operation)
        {
            ExecutionResult<TResult> response = await operation();

            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response.Result!);
        }

        protected async Task<ActionResult> ExecutionResultHandlerAsync(Func<Guid, Task<ExecutionResult>> operation)
        {
            if (!HttpContext.TryGetUserId(out Guid userId))
            {
                return BadRequest(new ExecutionResult("UnknowError", "Unknow error"));
            }

            ExecutionResult response = await operation(userId);

            if (!response.IsSuccess) return BadRequest(response);
            return NoContent();
        }
    }
}
