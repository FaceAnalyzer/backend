using System.Text;
using FaceAnalyzer.Api.Service.Providers;
using FaceAnalyzer.Api.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace FaceAnalyzer.Api.Service;

public static class IServiceCollectionExtensions
{
    public static void AddAppAuthentication(this IServiceCollection services, AppConfiguration config)
    {
        services.AddControllers()
            .AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter()));
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
}