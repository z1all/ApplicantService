using Microsoft.AspNetCore.Mvc;
using Common.Attributes;
using Common.Models;
using Common.DTOs;

namespace Common.Controllers
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
    }
}
