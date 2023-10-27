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
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var deletedAtProperty = entityType.FindProperty("DeletedAt");

            if (deletedAtProperty != null)
            {
                // Create a lambda expression to filter out entities with DeletedAt set.
                var parameter = Expression.Parameter(entityType.ClrType);
                var deletedAtExpression = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, deletedAtProperty.PropertyInfo),
                        Expression.Constant(null, deletedAtProperty.ClrType)
                    ),
                    parameter
                );

                // Set the filter for the entity type.
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(deletedAtExpression);
            }
        }
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Experiment> Experiments { get; set; }
    public DbSet<Stimuli> Stimuli { get; set; }
    public DbSet<Project> Projects { get; set; }
}