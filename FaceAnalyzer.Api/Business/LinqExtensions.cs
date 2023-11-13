using System.Linq.Expressions;
using FaceAnalyzer.Api.Business.Contracts;
namespace FaceAnalyzer.Api.Business;

public static class LinqExtensions
{
    public static QueryResult<T> ToQueryResult<T>(this IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        return new QueryResult<T>(list, list.Count);
    }

    public static IEnumerable<TSource> ConditionalWhere<TSource>(this IEnumerable<TSource> source,
        bool condition,
        Func<TSource, bool> predicate)
    {
    
        return condition ? source.Where(predicate) : source;
    }
}