using Microsoft.OpenApi.Models;
using Plitkarka.Domain.Services.ContextAccessToken;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Plitkarka.Application.Swagger;

public class AuthorizationHeaderSwaggerAttribute : IOperationFilter
{ 
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = ContextAccessTokenService.AuthorizationHeaderName,
            In = ParameterLocation.Header,
            Schema = new OpenApiSchema
            {
                Type = "string"
            }
        });
    }
}
