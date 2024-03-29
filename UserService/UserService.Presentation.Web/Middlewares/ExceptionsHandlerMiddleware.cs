using System.Collections.Immutable;
using System.Net;
using UserService.Core.Application.DTOs;

namespace UserService.Presentation.Web.Middlewares
{
    public class ExceptionsHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var status = (int)(ex switch
                { 
                    _ => HttpStatusCode.InternalServerError
                });

                httpContext.Response.StatusCode = status;
                httpContext.Response.ContentType = "application/json";

                ErrorResponse error = new()
                {
                    Status = status,
                    Title = "Unknow error(s)",
                    Errors = ImmutableDictionary<string, List<string>>.Empty.Add("UnknowError", ["Unknown error"]),
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
