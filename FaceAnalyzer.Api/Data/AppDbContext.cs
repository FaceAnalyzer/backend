using System.Linq.Expressions;
using FaceAnalyzer.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddAutoUpdatedAt();
        modelBuilder.AddAutoCreatedAt();
        modelBuilder.AddDeletedAtQueryFilter();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Experiment> Experiments { get; set; }
    public DbSet<Stimuli> Stimuli { get; set; }
    public DbSet<Project> Projects { get; set; }
}