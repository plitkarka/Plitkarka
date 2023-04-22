using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Services.ContextAccessToken;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Plitkarka.Application.Swagger;

public class AuthorizationHeaderSwaggerAttribute : IOperationFilter
{ 
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attrs = context.MethodInfo
            .GetCustomAttributes()
            .OfType<AuthorizeAttribute>()
            .ToList();

        if (attrs.Count == 0)
        {
            return;
        }

        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = ContextAccessTokenService.AuthorizationHeaderName,
            In = ParameterLocation.Header,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Description = "Token with specific lifetime needed to authorize user"
            }
        });
    }
}
