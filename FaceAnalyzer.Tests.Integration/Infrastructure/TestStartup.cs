using System.Text.Json.Serialization;
using FaceAnalyzer.Api;
using FaceAnalyzer.Api.Business;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Service;
using FaceAnalyzer.Api.Service.Middlewares;
using FaceAnalyzer.Api.Shared;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Security;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FaceAnalyzer.Tests.Integration.Infrastructure;

public class TestStartup
{
    private readonly bool _authenticationEnabled;

    public TestStartup(bool authenticationEnabled)
    {
        _authenticationEnabled = authenticationEnabled;
    }


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(ApiAssemblyMarker).Assembly)
            .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
        services.AddSingleton(AppConfiguration);
        services.AddBusinessModels();
        services.AddSingleton<AppDbContext, TestDbContext>(sp =>
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("FaceAnalyzerIntegrationTests")
                .Options;
            var config = sp.GetRequiredService<AppConfiguration>();
            return new TestDbContext(options, config);
        });
        services.AddMappers();
        services.AddMediatR();
        services.AddHttpContextAccessor();
        services.AddScoped<ErrorHandlerMiddleware>();

        services.AddAppAuthentication(AppConfiguration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseRouting();

        app.UseAuthentication();
        app.UseMiddleware<SetSecurityPrincipalMiddleware>();


        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
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