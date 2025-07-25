using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AddRefreshTokenHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-Refresh-Token",
            In = ParameterLocation.Header,
            Required = false,
            Description = "Refresh token used to renew access token when expired.",
            Schema = new OpenApiSchema
            {
                Type = "string"
            }
        });
    }
}
