using UserService.Presentation.Web.Middlewares;

namespace UserService.Presentation.Web
{
    internal static class PresentationWebMiddlewareExtensions
    {
        public static void UseExceptionsHandler(this WebApplication webApplication)
        {
            webApplication.UseMiddleware<ExceptionsHandlerMiddleware>();
        }
    }
}
