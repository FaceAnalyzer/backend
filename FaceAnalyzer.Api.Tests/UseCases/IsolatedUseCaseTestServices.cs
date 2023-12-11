using AutoMapper;
using FaceAnalyzer.Api.Business.UseCases;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Shared;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Security;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases;

public class IsolatedUseCaseTestServices<TUseCase> where TUseCase : BaseUseCase
{
    public IsolatedUseCaseTestServices(string dbName, params Profile[] profiles)
    {
        SetupMapper(profiles);
        SetupDbContext(dbName);
        var instance = (TUseCase?)Activator.CreateInstance(typeof(TUseCase), Mapper, DbContext);

        UseCase = instance ?? throw new Exception(
            $"Could not create instance of {typeof(TUseCase)} check the constructor of the target use case.");
    }

    public IMapper Mapper { get; private set; }
    public TUseCase UseCase { get; private set; }
    public AppDbContext DbContext { get; private set; }


    public AppConfiguration Configuration { get; private set; } = new AppConfiguration();

    public SecurityContext SecurityContext
    {
        get
        {
            SecurityContext context = new(Configuration);
            context.SetPrincipal(new SecurityPrincipal
            {
                Id = 1,
                Role = UserRole.Admin
            });
            return context;
        }
    }

    private void SetupMapper(params Profile[] profiles)
    {
        if (!profiles.Any())
        {
            throw new ArgumentNullException(nameof(profiles), "At least one profile must be provided. ");
        }

        var config = new MapperConfiguration(cfg => { cfg.AddProfiles(profiles); });
        Mapper = config.CreateMapper();
    }

    private void SetupDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"{dbName} - {Guid.NewGuid()}")
            .Options;
        DbContext = new AppDbContext(options, SecurityContext);
    }
}