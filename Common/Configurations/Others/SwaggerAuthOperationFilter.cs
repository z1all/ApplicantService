using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Configurations.Others
{
    internal class SwaggerAuthOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            bool thereIsAuthorize = context.MethodInfo.CustomAttributes.Any(attribute => attribute.AttributeType.Name.Contains(typeof(AuthorizeAttribute).Name));
            if (thereIsAuthorize)
            {
                operation.Security.Add(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    }
                );
            }

            return;
        }
    }
}
