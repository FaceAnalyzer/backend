using System.Reflection;
using System.Text;
using FaceAnalyzer.Api.Service.Middlewares;
using FaceAnalyzer.Api.Shared;
using FaceAnalyzer.Api.Shared.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service;

public static class IServiceCollectionExtensions
{
    public static void AddAppAuthentication(this IServiceCollection services, AppConfiguration config)
    {
        // services.AddControllers()
        //     .AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter()));
        services.AddScoped<SetSecurityPrincipalMiddleware>();
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            
            .AddJwtBearer(x =>
                x.TokenValidationParameters = new TokenValidationParameters{
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JwtConfig.Secret))
            });

        services.AddScoped<SecurityContext>();
    }


    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
     
 
        } );
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Face Analyzer", Version = "v1" });
    
            c.ExampleFilters();

            // c.OperationFilter<AddResponseHeadersFilter>(); // [SwaggerResponseHeader]

            // var filePath = Path.Combine(AppContext.BaseDirectory, "WebApi.xml");
            // c.IncludeXmlComments(filePath); // standard Swashbuckle functionality, this needs to be before c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>()

            // c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization
            // or use the generic method, e.g. c.OperationFilter<AppendAuthorizeToSummaryOperationFilter<MyCustomAttribute>>();

            // add Security information to each operation for OAuth2
            // c.OperationFilter<SecurityRequirementsOperationFilter>();
            // or use the generic method, e.g. c.OperationFilter<SecurityRequirementsOperationFilter<MyCustomAttribute>>();

            // // if you're using the SecurityRequirementsOperationFilter, you also need to tell Swashbuckle you're using OAuth2
            // c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            // {
            //     Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
            //     In = ParameterLocation.Header,
            //     Name = "Authorization",
            //     Type = SecuritySchemeType.ApiKey
            // });
        });
        services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
    }
}