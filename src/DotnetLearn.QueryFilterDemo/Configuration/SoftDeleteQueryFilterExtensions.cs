using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DotnetLearn.QueryFilterDemo.Configuration;

using EntityMetadata;

internal static class SoftDeleteExtensions
{
    const string DeletedDate = "DeletedOn";

    /// <summary>
    /// Configures soft delete column and query filter for entity
    /// </summary>
    /// <param name="entityTypeBuilder">Entity to configure on</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    internal static void AddSoftDeleteQueryFilter<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder)
        where TEntity : class, ISoftDeleteableEntity
    {
        // Shadow prop
        entityTypeBuilder.Property<DateTime?>(DeletedDate);
        entityTypeBuilder.HasQueryFilter(entity => EF.Property<DateTime?>(entity, DeletedDate) == null);
    }

    /// <summary>
    /// Configures soft delete column and query filter for entity
    /// </summary>
    /// <param name="entityTypeBuilder">Entity to configure on</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    internal static void AddSoftDeleteQueryFilters(this ModelBuilder modelBuilder)
    {
        foreach (var entity in
            from entityType in modelBuilder.Model.GetEntityTypes()
            where entityType.ClrType.IsAssignableTo(typeof(ISoftDeleteableEntity))
            select entityType
        )
        {
            // Shadow prop
            entity.AddProperty(DeletedDate, typeof(DateTime?));
            // Configure query filter, using the Expression Trees api
            var lambdaParameter = Expression.Parameter(entity.ClrType);

            var efPropertyMethod = typeof(EF)
                .GetMethod(nameof(EF.Property))
                ?.MakeGenericMethod(typeof(DateTime?))
                ?? throw new InvalidOperationException();;

            // Visualized: entity => EF.Property<DateTime?>(entity, DeletedDate) == null
            var queryFilterExpression =
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Call(null, efPropertyMethod, lambdaParameter, Expression.Constant(DeletedDate)),
                        Expression.Constant(null)
                    ),
                    lambdaParameter
                );
            entity.AddQueryFilter(queryFilterExpression);
        }
    }

    internal static void OverrideRemovedWithSoftDeleted(this ChangeTracker changeTracker, DateTime now)
    {
        foreach (var softDeleteRemovedEntity in
            from entityEntry in changeTracker.Entries()
            where entityEntry.State is EntityState.Deleted
            select entityEntry)
        {
            softDeleteRemovedEntity.State = EntityState.Modified;
            softDeleteRemovedEntity.Property(DeletedDate).CurrentValue = now;
        }
    }
}
