using Microsoft.Extensions.DependencyInjection;

namespace FaceAnalyzer.Api.Tests;

public class ServiceProviderFixture :  IDisposable, IAsyncDisposable, IServiceProvider
{
    private ServiceProvider _serviceProvider;
    private ServiceCollection _services;

    private ServiceProviderBuilder _builder = new ServiceProviderBuilder()
        .AddDefaults();
    public void AddTransient<T>() where T: class
    {
        _builder.AddTransient<T>();
    }
    
    public void Build()
    {
        _serviceProvider = _builder.Build();
    }
    
    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }
    
    public async ValueTask DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }
    
}