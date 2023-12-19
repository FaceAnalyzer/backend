using System.Reflection;
using System.Text;
using FaceAnalyzer.Api.Service.Middlewares;
using FaceAnalyzer.Api.Service.Swagger.Filters;
using FaceAnalyzer.Api.Shared;
using FaceAnalyzer.Api.Shared.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service;

public static class IServiceCollectionExtensions
{
    public static void AddAppAuthentication(this IServiceCollection services, AppConfiguration config)
    {
        services.AddControllers()
            .AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter()));
        services.AddScoped<SetSecurityPrincipalMiddleware>();
        services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
            .AddJwtBearer(x =>
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JwtConfig.Secret))
                });

        services.AddScoped<SecurityContext>();
    }


    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Face Analyzer", Version = "v1" });
            c.ExampleFilters();
            const string schemeName = JwtBearerDefaults.AuthenticationScheme;
            c.OperationFilter<AddSecurityRequirementFilter>(schemeName);
            c.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            c.EnableAnnotations();
        });
        services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
    }
}