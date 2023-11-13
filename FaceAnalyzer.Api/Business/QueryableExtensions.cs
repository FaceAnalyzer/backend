using System.Linq.Expressions;

namespace FaceAnalyzer.Api.Business;

public static class QueryableExtensions
{
    public static IQueryable<TSource> ConditionalWhere<TSource>(this IQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
}