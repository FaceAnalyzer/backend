using System.Text;
using FaceAnalyzer.Api.Service.Providers;
using FaceAnalyzer.Api.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FaceAnalyzer.Api.Service;

public static class IServiceCollectionExtensions
{
    public static void AddAppAuthentication(this IServiceCollection services, AppConfiguration config)
    {
        // services.AddControllers()
        //     .AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter()));
       
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

        services.AddScoped<AuthenticationManager>();
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
}