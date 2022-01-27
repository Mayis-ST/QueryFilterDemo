using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;

namespace DotnetLearn.QueryFilterDemo.Configuration;

internal static class QueryFilterExtensions
{
    internal static void AddQueryFilter(this EntityTypeBuilder entityType, LambdaExpression filter) =>
    AddQueryFilter(entityType.Metadata, filter);

    internal static void AddQueryFilter<TEntity>(this EntityTypeBuilder<TEntity> entityType, Expression<Func<TEntity, bool>> filter)
        where TEntity : class => AddQueryFilter(entityType.Metadata, filter);

    internal static void AddQueryFilter(this IMutableEntityType mutableEntityType, LambdaExpression filter)
    {
        var currentQueryFilter = mutableEntityType.GetQueryFilter();
        if (currentQueryFilter is null) // if no query filter is present, no further processing is necessary
        {
            mutableEntityType.SetQueryFilter(filter);
            return;
        }

        var currentFilterParameter = currentQueryFilter.Parameters.Single();

        // replace usages of the new filter parameter with defined query filter parameters
        // e.g. if there's  a query filter defined as a => a.SomeProp == null
        // and we add a new query filter b => b.SomeOtherProp >= 10
        // the result of calling this method on the second expr tree will be
        // a.SomeOtherProp >= 10
        var fixedNewQueryFilterBody = ReplacingExpressionVisitor.Replace(
            original: filter.Parameters.Single(),
            replacement: currentFilterParameter,
            tree: filter.Body);

        // now we just need to extract the body of the currently configured query filter
        // and set the new query filter to be a lambda expr of the following form
        // entity => currentFilter.Body && fixedNewFilterBody
        mutableEntityType.SetQueryFilter(
            Expression.Lambda(
                Expression.AndAlso(
                    currentQueryFilter.Body,
                    fixedNewQueryFilterBody
                ),
                currentFilterParameter
            ));
    }
}