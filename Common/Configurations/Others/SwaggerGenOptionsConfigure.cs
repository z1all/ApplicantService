using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Configurations.Others
{
    public class SwaggerGenOptionsConfigure : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                                "Enter 'Your token in the text input below.\r\n\r\n" +
                                "Example: \"eyJhbGciOiJIUzI1NiJ9.eyJwYXlsb2FkIjoi0JLQvtC_0YDQvtGBLCDQl9CQ0KfQldCcPz8_In0.lyOs-Vq66shvnDET9eAQ_9pjhxhwkqf8B_9hhOuq8Yc\"",
            });

            options.OperationFilter<SwaggerAuthOperationFilter>();
        }
    }
}
