namespace FaceAnalyzer.Api.Business.Contracts;

public record QueryResult<T>(IList<T> Items, int Count);