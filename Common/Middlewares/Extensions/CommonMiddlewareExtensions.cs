using Microsoft.AspNetCore.Builder;

namespace Common.Middlewares.Extensions
{
    public static class CommonMiddlewareExtensions
    {
        public static void UseExceptionsHandler(this WebApplication webApplication)
        {
            webApplication.UseMiddleware<ExceptionsHandlerMiddleware>();
        }
    }
}
