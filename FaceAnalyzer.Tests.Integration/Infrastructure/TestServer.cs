using System.Text.Json.Serialization;
using FaceAnalyzer.Api.Business;
using FaceAnalyzer.Api.Service;
using FaceAnalyzer.Api.Service.Middlewares;
using FaceAnalyzer.Api.Shared;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Microsoft.AspNetCore.Hosting.IWebHostBuilder;

namespace FaceAnalyzer.Tests.Integration.Infrastructure;

public class TestHostFixture : IDisposable
{
    public IHost Host = new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
            webBuilder
                .UseTestServer()
                .UseStartup<TestStartup>();
        })
        .Build();

    public void Dispose()
    {
        Host.Dispose();
    }
}