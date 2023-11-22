using FaceAnalyzer.Api.Business;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Shared;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FaceAnalyzer.Api.Tests;

public class ServiceProviderBuilder
{
    private readonly ServiceCollection _services = new ();
    public ServiceProviderBuilder AddDefaults()
    {
       
        _services.AddMappers();
        
        _services.AddSingleton(AppConfiguration);
        var securityContext = new SecurityContext(AppConfiguration);
        securityContext.SetPrincipal(new SecurityPrincipal
        {
            Id = -1,
            Role = UserRole.Admin
        });
        
        _services.AddSingleton(securityContext);
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("FaceAnalyzerTests")
            .Options;

        _services.AddTransient<AppDbContext>(
            _ => new AppDbContext(options, securityContext)
        );
        
        return this;
    }

    public ServiceProviderBuilder AddTransient<T>() where T: class
    {
        _services.AddTransient<T>();
        return this;
    }

    public ServiceProvider Build()
    {
        return _services.BuildServiceProvider();
    }

    private static AppConfiguration AppConfiguration
    {
        get
        {
            var connectionStrings = new ConnectionStrings
            {
                AppDatabase = "",
                DbVersion = ""
            };
            var jwtConfig = new JwtConfig();

            var config = new AppConfiguration
            {
                ConnectionStrings = connectionStrings,
                JwtConfig = jwtConfig
            };
            return config;
        }
    }
}