using FaceAnalyzer.Api.Business.Contracts;

namespace FaceAnalyzer.Api.Business;

public static class LinqExtensions
{
    public static QueryResult<T> ToQueryResult<T>(this IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        return new QueryResult<T>(list, list.Count);
    }
    
}