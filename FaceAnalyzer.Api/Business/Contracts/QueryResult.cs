using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Contracts;

[SwaggerSchema(Title = nameof(QueryResult<T>))]
public record QueryResult<T>(IList<T> Items, int Count);