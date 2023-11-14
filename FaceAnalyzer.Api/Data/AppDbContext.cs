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
        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.Username, u.DeletedAt })
            .IsUnique();
        
        modelBuilder.AddAutoUpdatedAt();
        modelBuilder.AddAutoCreatedAt();
        modelBuilder.AddDeletedAtQueryFilter();
    }

    public void Delete<TEntity>(TEntity entity)  where TEntity : IDeletable
    {
        entity.DeletedAt = DateTime.Now;
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Experiment> Experiments { get; set; }
    public DbSet<Stimuli> Stimuli { get; set; }
    public DbSet<Project> Projects { get; set; }

    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<Emotion> Emotions { get; set; }
}