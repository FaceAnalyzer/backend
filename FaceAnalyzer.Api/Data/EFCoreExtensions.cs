using System.Linq.Expressions;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Security;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Data;

public static class EFCoreExtensions
{
    public static void AddDeletedAtQueryFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var deletedAtPropertyName = nameof(IDeletable.DeletedAt);
            var deletedAtProperty = entityType.FindProperty(deletedAtPropertyName);

            if (deletedAtProperty != null)
            {
                // (IDeletable entity) => entity.DeletedAt == null
                var parameter = Expression.Parameter(entityType.ClrType);
                var deletedAtExpression = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, deletedAtProperty.PropertyInfo),
                        Expression.Constant(null, deletedAtProperty.ClrType)
                    ),
                    parameter
                );

                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(deletedAtExpression);
            }
        }
    }

    public static void AddAutoCreatedAt(this ModelBuilder modelBuilder)
    {
        const string createdAtPropertyName = nameof(EntityBase.CreatedAt);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var createdAt = entityType
                .FindProperty(createdAtPropertyName);
            if (createdAt != null)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(createdAtPropertyName)
                    .HasColumnType("TIMESTAMP")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            }
        }
    }

    public static void AddAutoUpdatedAt(this ModelBuilder modelBuilder)
    {
        const string updatedAtPropertyName = nameof(EntityBase.UpdatedAt);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var updatedAtProperty = entityType.FindProperty(updatedAtPropertyName);
            if (updatedAtProperty != null)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(updatedAtPropertyName)
                    .HasColumnType("TIMESTAMP")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            }
        }
    }
}