using FaceAnalyzer.Api.Business.UseCases;
using FaceAnalyzer.Api.Data;
using Microsoft.Extensions.DependencyInjection;

namespace FaceAnalyzer.Api.Tests.UseCases;

public class UseCaseTestBase<TUseCase> : IClassFixture<ServiceProviderFixture> where TUseCase : BaseUseCase
{
    protected TUseCase UseCase { get; }
    protected AppDbContext DbContext { get; }
    protected ServiceProviderFixture ServiceProviderFixture { get; }
    
    public void CleanDatabase()
    {
        DbContext.Database.EnsureDeleted();
    }
   

    protected void AddServices(ServiceProviderFixture fixture)
    {
        fixture.AddTransient<TUseCase>();
    }
    public UseCaseTestBase(ServiceProviderFixture serviceProviderFixture)
    {
        ServiceProviderFixture = serviceProviderFixture;
        AddServices(ServiceProviderFixture);
        ServiceProviderFixture.Build();
        UseCase =  ServiceProviderFixture.GetService<TUseCase>() 
                   ?? throw new Exception("Use Case was not found in the service container");
        DbContext = ServiceProviderFixture.GetService<AppDbContext>() 
                    ?? throw new Exception("Db Context was not found in the service container");
    }
}