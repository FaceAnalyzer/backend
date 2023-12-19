using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Shared;
using FaceAnalyzer.Api.Shared.Security;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Tests.Integration.Infrastructure;

public class TestDbContext : AppDbContext
{
    public TestDbContext(DbContextOptions<AppDbContext> options, AppConfiguration config) : base(options, new SecurityContext(config))
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Console.WriteLine("TestDbContext.OnModelCreating");
    }
}