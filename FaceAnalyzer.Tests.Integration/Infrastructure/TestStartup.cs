using System.Text.Json.Serialization;
using FaceAnalyzer.Api;
using FaceAnalyzer.Api.Business;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Service;
using FaceAnalyzer.Api.Service.Middlewares;
using FaceAnalyzer.Api.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FaceAnalyzer.Tests.Integration.Infrastructure;

public class TestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(ApiAssemblyMarker).Assembly)
            .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
        services.AddSingleton(AppConfiguration);
        services.AddBusinessModels();
        services.AddDbContext<AppDbContext>(opt => { opt.UseInMemoryDatabase("FaceAnalyzerIntegrationTests"); });
        services.AddAppAuthentication(AppConfiguration);
        services.AddMappers();
        services.AddHttpContextAccessor();
        services.AddMediatR();
        services.AddScoped<ErrorHandlerMiddleware>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();
        app.UseCors(opt =>
        {
            opt.AllowAnyHeader();
            opt.AllowAnyOrigin();
            opt.AllowAnyMethod();
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<SetSecurityPrincipalMiddleware>();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.UseMiddleware<ErrorHandlerMiddleware>();
    }

    private AppConfiguration AppConfiguration
    {
        get
        {
            var connectionStrings = new ConnectionStrings();
            var jwtConfig = new JwtConfig
            {
                Expiry = 100000,
                Secret = "1234567890123456"
            };

            var config = new AppConfiguration
            {
                ConnectionStrings = connectionStrings,
                JwtConfig = jwtConfig
            };
            return config;
        }
    }
}