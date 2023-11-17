using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FaceAnalyzer.Api.Service.Swagger.Filters;

public class AddSecurityRequirementFilter : IOperationFilter
{
    private readonly string _securitySchemeName;

    public AddSecurityRequirementFilter(string securitySchemeName)
    {
        _securitySchemeName = securitySchemeName;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attributes = context.GetControllerAndActionAttributes<AllowAnonymousAttribute>();
        if (attributes.Any())
        {
            return;
        }

        AddSecurity(operation);
        AddRoles(operation, context);
    }

    private void AddRoles(OpenApiOperation operation, OperationFilterContext context)
    {
        var attributes = context.GetControllerAndActionAttributes<AuthorizeAttribute>();

        if (!attributes.Any())
        {
            return;
        }

        var roles = new List<string>();
        foreach (var attribute in attributes)
        {
            if (!string.IsNullOrEmpty(attribute.Roles))
            {
                roles.Add(attribute.Roles);
            }
        }

        if (roles.Any())
        {
            operation.Summary += "  Allowed Roles: " + string.Join(", ", roles);
        }
    }

    private void AddSecurity(OpenApiOperation operation)
    {
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                        { Type = ReferenceType.SecurityScheme, Id = _securitySchemeName }
                },
                new List<string>()
            }
        });
    }
}