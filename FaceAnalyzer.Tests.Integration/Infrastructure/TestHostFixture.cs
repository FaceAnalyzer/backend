using System.Text.Json.Serialization;
using CsvHelper.Configuration;
using FaceAnalyzer.Api;
using FaceAnalyzer.Api.Business;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Service;
using FaceAnalyzer.Api.Service.Middlewares;
using FaceAnalyzer.Api.Shared;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FaceAnalyzer.Tests.Integration.Infrastructure;

public class TestHostFixture : IAsyncDisposable
{
    public IServiceProvider ServiceProvider => _host.Services;

    private bool AuthenticationEnabled;
    private IHost _host;
    private SecurityPrincipal _securityPrincipal;

    public void Build()
    {
        _host = new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .UseStartup((_) => new TestStartup(AuthenticationEnabled));
            })
            .Build();
    }


    public TestHostFixture AddDefaultPrincipal(int id, UserRole role)
    {
        _securityPrincipal = new SecurityPrincipal
        {
            Id = id,
            Role = role
        };
        return this;
    }

    public TestHostFixture EnableAuthentication()
    {
        AuthenticationEnabled = true;
        return this;
    }

    public Task StartHost()
    {
        if (_host is null)
        {
            throw new ConfigurationException("Web host is not built, call Build on TestHostFixture first.");
        }

        return _host.StartAsync();
    }

    public T GetService<T>() where T : class
    {
        if (_host is null)
        {
            throw new ConfigurationException("Web host is not built, call Build on TestHostFixture first.");
        }

        return _host.Services.GetRequiredService<T>();
    }

    public HttpClient GetClient()
    {
        if (_host is null)
        {
            throw new ConfigurationException("Web host is not built, call Build on TestHostFixture first.");
        }

        var client = _host.GetTestClient();
        if (AuthenticationEnabled)
        {
            var securityContext = _host.Services.GetRequiredService<SecurityContext>();
            var token = securityContext.CreateJwt(_securityPrincipal);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        return client;
    }

    public async ValueTask DisposeAsync()
    {
        await _host.StopAsync();
        _host.Dispose();
    }
}