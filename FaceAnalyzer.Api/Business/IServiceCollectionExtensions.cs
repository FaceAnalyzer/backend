using FaceAnalyzer.Api.Business.BusinessModels;
using FaceAnalyzer.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business;

public static class IServiceCollectionExtensions
{
    public static void AddBusinessModels(this IServiceCollection services)
    {
        var businessModels = typeof(IServiceCollectionExtensions).Assembly.GetTypes()
            .Where(t => t.IsClass)
            .Where(t => t.IsSubclassOf(typeof(BusinessModelBase)));

        foreach (var item in businessModels)
        {
            services.AddScoped(item);
        }
    }

    public static void AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BusinessModelBase).Assembly);
    }

    public static void AddDbContexts(this IServiceCollection services, string connectionString, string dbVersion)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            // var connectionString = "server=localhost;user=root;password=1234;database=ef";
            var serverVersion = new MySqlServerVersion(new Version(dbVersion));
            opt.UseMySql(connectionString, serverVersion);
        });
    }

    public static void AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<Program>());
    }
}