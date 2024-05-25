using Microsoft.AspNetCore.Mvc;
using Common.API.Attributes;
using Common.API.DTOs;
using Common.API.Helpers;
using Common.Models.Models;

namespace Common.API.Controllers
{
    [ApiController]
    [ValidateModelState]
    public abstract class BaseController : ControllerBase
    {
        protected ObjectResult ExecutionResultHandlerAsync(ExecutionResult executionResult, string? otherMassage = null)
        {
            //return BadRequest(new ErrorResponse()
            //{
            //    Title = otherMassage ?? "One or more errors occurred.",
            //    Status = 400,
            //    Errors = executionResult.Errors,
            //});

            return StatusCode((int)executionResult.StatusCode, new ErrorResponse()
            {
                Title = otherMassage ?? "One or more errors occurred.",
                Status = (int)executionResult.StatusCode,
                Errors = executionResult.Errors,
            });
        }

        protected async Task<ActionResult<TResult>> ExecutionResultHandlerAsync<TResult>(Func<Guid, Task<ExecutionResult<TResult>>> operation)
        {
            if (!HttpContext.TryGetUserId(out Guid userId))
            {
                return ExecutionResultHandlerAsync(new ExecutionResult(StatusCodeExecutionResult.InternalServer, "UnknowError", "Unknow error"));
            }

            ExecutionResult<TResult> response = await operation(userId);

            if (!response.IsSuccess) return ExecutionResultHandlerAsync(response);
            return Ok(response.Result!);
        }

        protected async Task<ActionResult<TResult>> ExecutionResultHandlerAsync<TResult>(Func<Task<ExecutionResult<TResult>>> operation)
        {
            ExecutionResult<TResult> response = await operation();

            if (!response.IsSuccess) return ExecutionResultHandlerAsync(response);
            return Ok(response.Result!);
        }

        protected async Task<ActionResult> ExecutionResultHandlerAsync(Func<Guid, Task<ExecutionResult>> operation)
        {
            if (!HttpContext.TryGetUserId(out Guid userId))
            {
                return ExecutionResultHandlerAsync(new ExecutionResult(StatusCodeExecutionResult.InternalServer, "UnknowError", "Unknow error"));
            }

            ExecutionResult response = await operation(userId);

            if (!response.IsSuccess) return ExecutionResultHandlerAsync(response);
            return NoContent();
        }
    }
}
