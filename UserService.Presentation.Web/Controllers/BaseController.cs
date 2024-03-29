using Microsoft.AspNetCore.Mvc;
using UserService.Core.Application.DTOs;
using UserService.Presentation.Web.Attributes;
using Common.Models;

namespace UserService.Presentation.Web.Controllers
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
