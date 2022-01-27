namespace DotnetLearn.QueryFilterDemo.Configuration;

using System.Linq.Expressions;
using EntityMetadata;

internal static class TenantQueryFilterExtensions
{
    /// <summary>
    /// Configures a tenant id query filter for entity
    /// </summary>
    /// <param name="entityTypeBuilder">Entity to configure on</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    internal static void AddTenantQueryFiltersBad(this ModelBuilder modelBuilder, int tenantId)
    {
        foreach (var tenantSpecificEntity in
            from entity in modelBuilder.Model.GetEntityTypes()
            where entity.ClrType.IsAssignableTo(typeof(ITenantSpecific))
            select entity)
            {
                var queryFilterLambdaParameter = Expression.Parameter(tenantSpecificEntity.ClrType);

                var queryFilterBody = Expression.Equal(
                    Expression.Property(queryFilterLambdaParameter, nameof(ITenantSpecific.TenantId)),
                    Expression.Constant(tenantId)
                );

                tenantSpecificEntity.AddQueryFilter(Expression.Lambda(queryFilterBody, queryFilterLambdaParameter));
            }
    }
    /// <summary>
    /// Configures a tenant id query filter for entity
    /// </summary>
    /// <param name="entityTypeBuilder">Entity to configure on</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    internal static void AddTenantQueryFilters(this ModelBuilder modelBuilder, DbContext context)
    {
        foreach (var tenantSpecificEntity in
            from entity in modelBuilder.Model.GetEntityTypes()
            where entity.ClrType.IsAssignableTo(typeof(ITenantSpecific))
            select entity)
            {
                var queryFilterLambdaParameter = Expression.Parameter(tenantSpecificEntity.ClrType);

                var queryFilterBody = Expression.Equal(
                    Expression.Property(queryFilterLambdaParameter, nameof(ITenantSpecific.TenantId)),
                    Expression.Field(Expression.Constant(context), "tenantId")
                );

                tenantSpecificEntity.AddQueryFilter(Expression.Lambda(queryFilterBody, queryFilterLambdaParameter));
            }
    }
}